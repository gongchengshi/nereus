using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFirst.Models
{
    public class Tag
    {
        public Tag()
        {
            this.Users = new List<User>();
            this.Urls = new List<Url>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Url> Urls { get; set; }

    }
}
