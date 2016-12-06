using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Nereus.Utils;

namespace Nereus.Models
{
   public class Project
   {
      public Project()
      {
         Users = new List<User>();
      }

      public int Id { get; set; }
      public string Name { get; set; }

      public List<User> Users { get; set; }
      [Display(Name="Private")]
      public bool IsPrivate { get; set; }
      public bool IsArchived { get; set; }

      public int SearchProviderId { get; set; }
      public SearchProvider SearchProvider { get; set; }
   }

   public class ProjectUrl
   {
      public ProjectUrl() { }

      public ProjectUrl(Project project, Url url)
      {
         Project = project;
         Url = url;
      }

      public long Id { get; set; }

      public bool Irrelevant { get { return Rating == -1; } }
      public bool Hidden { get; set; }
      public int Rating { get; set; }

      public long UrlId { get; set; }
      public Url Url { get; set; }

      public int ProjectId { get; set; }
      public Project Project { get; set; }

      public bool HasIrrelevantPattern
      {
         get
         {
            return Globals.CompiledIrrelevantUrlPatterns[ProjectId].Values.Any(pattern => pattern.IsMatch(Url.Path));
         }
      }

      public bool HasHiddenPattern
      {
         get
         {
            return Globals.CompiledHiddenUrlPatterns[ProjectId].Values.Any(pattern => pattern.IsMatch(Url.Path));
         }
      }
   }

   public class ProjectUrlPattern
   {
      public ProjectUrlPattern() { }

      public ProjectUrlPattern(int projectId, string pattern)
      {
         ProjectId = projectId;
         Pattern = pattern;
      }

      public long Id { get; set; }

      [Required]
      public int ProjectId { get; set; }
      public Project Project { get; set; }

      [Required]
      public string Pattern { get; set; }

      [Required]
      public bool Hidden { get; set; }

      [Required]
      public bool Irrelevant { get; set; }
   }

   public class ProjectQuery
   {
      public ProjectQuery() { }

      public ProjectQuery(Project project, string text, DateTime lastUsed)
      {
         Project = project;
         Text = text;
         LastUsed = lastUsed;
         
      }

      public long Id { get; set; }

      public int ProjectId { get; set; }
      public Project Project { get; set; }

      [Required]
      public string Text { get; set; }
      [Required]
      public DateTime LastUsed { get; set; }
   }
}
