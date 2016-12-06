using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Mustache;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebMining.SearchProvidor;
using Gongchengshi.Collections;
using Gongchengshi.Web;

namespace Nereus.Atrax
{
    public class AtraxSearch
   {
      private readonly ElasticsearchClient _client;
      private readonly string _index;

      public AtraxSearch(string jobName)
      {
         _index = jobName;
         //var node = new Uri("http://10.16.45.170:9200");
         var node = new Uri("http://lpul-atraxidx1:9200");
         var config = new ConnectionConfiguration(node).ExposeRawResponse();
         _client = new ElasticsearchClient(config);
      }

      public SearchResults Search(string q, int start = 0, int count = 10, string lang="en", bool googlebotBlockedOnly=false, bool showDuplicates=false)
      {
         var hbs = new FormatCompiler();
         var generator = hbs.Compile(Encoding.UTF8.GetString(Properties.Resources.BasicQuery));
         var query = generator.Render(new
         {
            query_text = q,
            from = start,
            size = count,
            language = lang,
            googlebot_blocked = googlebotBlockedOnly.ToString().ToLower()
         });

         var response = _client.Search(_index, "text_document", query);

         //var jsonResponse = Encoding.UTF8.GetString(response.ResponseRaw); // For inspecting the raw response.
         var reader = new JsonTextReader(new StreamReader(new MemoryStream(response.ResponseRaw), Encoding.UTF8));
         var o = JObject.Load(reader);
         var hits = o.SelectToken("hits");

         var results = new SearchResults(q, count, int.Parse(hits.SelectToken("total").ToString()), start);

         int batchOrdinal = 0;
         var hitsHits = hits.SelectToken("hits");

         var similiarityHashDigests = new List<ulong>();
         var duplicateCount = 0;

         foreach (var hit in hitsHits)
         {
            if (!showDuplicates)
            {
               var similiarityHashDigest = (ulong)Convert.ToInt64(hit.SelectToken("fields.similarity_hash_digest").First.ToString());
               if (similiarityHashDigests.FindByHammingDistance(similiarityHashDigest, 2).Any())
               {
                  ++duplicateCount;
                  continue;
               }
               similiarityHashDigests.Add(similiarityHashDigest);
            }

            var url = (string)hit.SelectToken("fields.url").First;

            var titleObject = hit.SelectToken("fields.title");
            var title = titleObject == null ? null : titleObject.First.ToString();

            var googlebotBlockedObject = hit.SelectToken("fields.googlebot_blocked");
            var googlebotBlocked = googlebotBlockedObject != null && bool.Parse(googlebotBlockedObject.First.ToString());

            var highlightedTitleObject = hit.SelectToken("highlight.title");
            var htmlTitle = highlightedTitleObject == null ? null : highlightedTitleObject.First.ToString();

            var highlightedUrlObject = hit.SelectToken("highlight.url");
            var htmlUrl = highlightedUrlObject == null ? null : highlightedUrlObject.First.ToString();

            var highlightedBodyObject = hit.SelectToken("highlight.body");
            var htmlSnippet = highlightedBodyObject == null ? null : string.Join("<br>", highlightedBodyObject.Select(s => (string) s));

            string snippet = null;
            if (htmlSnippet != null)
            {
               htmlSnippet = Regex.Replace(htmlSnippet, "<!--", "");
               snippet = HtmlRemoval.StripTags(htmlSnippet);
            }

            results.Add(new SearchResult
            {
               BatchOrdinal = batchOrdinal++,
               QueryOrdinal = start + batchOrdinal,
               Url = url,
               CacheUrl = url,
               Snippet = snippet,
               HtmlSnippet = htmlSnippet,
               Title = title,
               HtmlTitle = htmlTitle ?? title,
               HtmlUrl = htmlUrl ?? url,
               GoogleBotBlocked = googlebotBlocked
            });
         }

         results.NumDuplicateResults = duplicateCount;
         return results;
      }
   }
}
