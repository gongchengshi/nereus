using System;
using System.Linq;
using System.Web.Mvc;
using CFirst.Models;
using System.Data.Entity;
using System.Net;
using Gongchengshi.Collections.Generic;

namespace CFirst.Controllers
{
    public class UrlController : Controller
    {
        //
        // GET: /Url/
        private CFirstModelsContext _database = new CFirstModelsContext();
        
        public ActionResult Index()
        {
            //_database.Configuration.LazyLoadingEnabled = false;
            var urlM = new CFirst.ViewModels.UrlViewModel();
            var site = Request.Url.AbsoluteUri;

            var user = _database.GetUser("Jane"); //Must be updated for release

            urlM.Address = site;
            urlM.Tags = _database.Tags.ToList();
            urlM.Comments = _database.Comments.Where(x => x.Url.Address == site).ToList();
            urlM.Comments = _database.Comments.Include(x => x.User).ToList();

            foreach (var comment in urlM.Comments)
            {
                _database.Entry(comment).Reference(x => x.User).Load();
            }
            foreach (var tag in urlM.Tags)
            {
                _database.Entry(tag).Collection(x => x.Users).Load();
            }
            urlM.User = user;
            
            
            return View(urlM);
        }

        [HttpPut]
        public HttpStatusCodeResult AddTag(Tag tag, Url url)
        {
            var user = _database.GetUser("Jane"); //Test user
            var urlObj = _database.Urls.FirstOr(x => x.Address == url.Address, () => { _database.Urls.Add(url); return url; });
            var tagObj = _database.Tags.FirstOrDefault(x => x.Name == tag.Name);

            if (tagObj == null)
            {
                _database.Tags.Add(tag);
                tagObj = tag;
            }
            else
            {
                _database.Entry(tagObj).Collection(x => x.Urls).Load();
                _database.Entry(tagObj).Collection(x => x.Users).Load();
            }
            
            tagObj.Users.IfNotContainsAdd(user);
            tagObj.Urls.IfNotContainsAdd(urlObj);

            _database.SaveChanges(); 

            _database.Entry(user).Collection(x => x.Urls).Load();
            user.Urls.IfNotContainsAdd(urlObj);

            _database.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpStatusCodeResult AddComment(Comment comment, Url url)
        {
            
            var user = _database.GetUser("Jane"); //Test user, update for release

            comment.Created = DateTime.Today;
            comment.Url = url;
            comment.User = user;

            var urlObj = _database.Urls.FirstOrDefault(x => x.Address == url.Address);
            
            if (urlObj == null)
            {
                _database.Urls.Add(url);
                urlObj = url;
            }
            else
            {
                 _database.Entry(urlObj).Collection(x => x.Users).Load();
            }

            urlObj.Users.IfNotContainsAdd(user);

            var commentObj = _database.Comments.FirstOrDefault(x => x.Content == comment.Content);
            if (commentObj == null)
            {
                _database.Comments.Add(comment);
                commentObj = comment;
            }
            else
            {
                _database.Entry(commentObj).Reference(x => x.User).Load();
                _database.Entry(commentObj).Reference(x => x.Url).Load();
            }

            _database.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}
