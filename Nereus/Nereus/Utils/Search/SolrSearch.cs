using System;
using Nereus.Models;
using Nereus.ViewModels;
using WebMining.SearchProvidor;

namespace Nereus.Utils.Search
{
    public class SolrSearch : ISearchProvider
    {
        private readonly WebMining.SearchProvidor.Solr.SolrSearch _solr;

        public SolrSearch(string indexName)
        {
            _solr = new WebMining.SearchProvidor.Solr.SolrSearch(indexName);
        }

        public Tuple<string, SearchResults> Search(string q, int start = 0, int count = 10, UserSearchSettings settings = null, TemporarySearchSettings tempSettings = null, SearchProfile profile = null)
        {
            var searchResults = NormalizeSearchResults.FromSolr(_solr.Search(q, start, count), q, start);
            return Tuple.Create(_solr.IndexName, searchResults);
        }
    }
}
