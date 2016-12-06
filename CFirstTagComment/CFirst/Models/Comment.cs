using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFirst.Models
{
    public class Comment
    {

        public int Id { get; set; }
        public string Content { get; set; }
        public Url Url { get; set; }
        public System.DateTime Created { get; set; }
        public User User { get; set; }
    }
}