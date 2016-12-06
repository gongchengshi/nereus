using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFirst.Models
{
    public class User
    {
        public User()
        {
            this.Tags = new List<Tag>();
            this.Urls = new List<Url>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Url> Urls { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
