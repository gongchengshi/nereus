using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CFirst.Models;

namespace Nereus.TagsComments.Models
{
    public class TagsCommentsModel 
    {
            
        public TagsCommentsModel()
        {
            Tags = new List<Tag>();
            Comments = new List<Comment>();
        }

        public User User { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
