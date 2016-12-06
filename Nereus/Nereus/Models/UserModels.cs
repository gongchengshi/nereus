using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nereus.Models
{
   public class User
   {
      public int Id { get; set; }
      public string UserName { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string FullName { get { return FirstName + " " + LastName; } }
      //[ForeignKey("Id")]
      public List<Project> Projects { get; set; }
      
   }

   public class UserUrl
   {
      public long Id { get; set; }

      public int UserId { get; set; }
      public virtual User User { get; set; }

      public long UrlId { get; set; }
      public virtual Url Url { get; set; }

      public DateTime? LastViewed
      {
         get { return _LastViewed; }
         set
         {
            PrevLastViewed = _LastViewed;
            _LastViewed = value;
         }
      }
      private DateTime? _LastViewed;
      public DateTime? PrevLastViewed { get; set; }
   }

   public class UserProjectUrl
   {
      public UserProjectUrl() { }

      public UserProjectUrl(ProjectUrl projectUrl, User user)
      {
         ProjectUrl = projectUrl;
         User = user;
      }

      public long Id { get; set; }

      public long ProjectUrlId { get; set; }
      public ProjectUrl ProjectUrl { get; set; }

      public int UserId { get; set; }
      public User User { get; set; }
   }

   public class UserSearchProfile
   {
      public long Id { get; set; }

      public int UserId { get; set; }
      public User User { get; set; }

      public int SearchProfileId { get; set; }
      public SearchProfile SearchProfile { get; set; }
   }

   public class UserSearchSettings
   {
       public UserSearchSettings()
       {
           // Default values
           NumResultsPerPage = 10;
           ResultsColumnWidth = int.MaxValue;
       }

       public long Id { get; set; }

       public int UserId { get; set; }
       public virtual User User { get; set; }

       public int NumResultsPerPage { get; set; }

       public int ResultsColumnWidth { get; set; }

   }
}
