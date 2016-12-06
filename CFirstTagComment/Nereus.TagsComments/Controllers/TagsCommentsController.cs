using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CFirst.Models;
using Nereus.TagsComments.Models;

namespace Nereus.TagsComments.Controllers
{
    public class TagsCommentsController : Controller
    {
        //
        // GET: /TagsComments/
        private CFirstModelsContext _database = new CFirstModelsContext();

        public ActionResult Index()
        {
            TagsCommentsModel tcM = new Models.TagsCommentsModel();

            tcM.Tags = _database.Tags;
            tcM.Comments = _database.Comments;
            tcM.User = _database.GetUser("Jane");
            return View(tcM);
        }

        //public ActionResult Index()
        //{
        //    //_database.Configuration.LazyLoadingEnabled = false;
        //    var urlM = new CFirst.ViewModels.UrlViewModel();
        //    var site = Request.Url.AbsoluteUri;

        //    var user = _database.GetUser("Jane"); //Must be updated for release

        //    urlM.Address = site;
        //    urlM.Tags = _database.Tags.ToList();
        //    urlM.Comments = _database.Comments.Where(x => x.Url.Address == site).ToList();
        //    urlM.Comments = _database.Comments.Include(x => x.User).ToList();

        //    foreach (var comment in urlM.Comments)
        //    {
        //        _database.Entry(comment).Reference(x => x.User).Load();
        //    }
        //    foreach (var tag in urlM.Tags)
        //    {
        //        _database.Entry(tag).Collection(x => x.Users).Load();
        //    }
        //    urlM.User = user;


        //    return View(urlM);
        //}

        //[HttpPut]
        //public HttpStatusCodeResult AddTag(Tag tag, Url url)
        //{
        //    var user = _database.GetUser("Jane"); //Test user
        //    var urlObj = _database.Urls.FirstOr(x => x.Address == url.Address, () => { _database.Urls.Add(url); return url; });
        //    var tagObj = _database.Tags.FirstOrDefault(x => x.Name == tag.Name);

        //    if (tagObj == null)
        //    {
        //        _database.Tags.Add(tag);
        //        tagObj = tag;
        //    }
        //    else
        //    {
        //        _database.Entry(tagObj).Collection(x => x.Urls).Load();
        //        _database.Entry(tagObj).Collection(x => x.Users).Load();
        //    }

        //    tagObj.Users.IfNotContainsAdd(user);
        //    tagObj.Urls.IfNotContainsAdd(urlObj);

        //    _database.SaveChanges();

        //    _database.Entry(user).Collection(x => x.Urls).Load();
        //    user.Urls.IfNotContainsAdd(urlObj);

        //    _database.SaveChanges();

        //    return new HttpStatusCodeResult(HttpStatusCode.OK);
        //}
    }
}
