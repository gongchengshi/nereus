using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CFirst.Models;

namespace CFirst.ViewModels
{
    public class UrlViewModel 
    {
        public UrlViewModel()
        {
            Tags = new List<Tag>();
            Comments = new List<Comment>();
        }
        public User User { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Comment> Comments { get; set; }
        public string Address { get; set; }
    }
}