using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFirst.Models
{
    public class Url
    {
        public Url()
        {
            this.Users = new List<User>();
            this.Tags = new List<Tag>();
        }


        public int Id { get; set;}
        public string Address { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }

    

}