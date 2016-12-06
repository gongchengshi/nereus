using System.Collections.Generic;
using Nereus.Models;

namespace Nereus.ViewModels
{
   public class ProjectBasedViewModel
   {
      public string ProjectName { get; set; }   
   }

   public class QueriesViewModel : ProjectBasedViewModel
   {
      public IEnumerable<ProjectQuery> Queries { get; set; }
   }

   public enum PathType
   {
      Hidden,
      Irrelevant
   }

   public class PathsViewModel : ProjectBasedViewModel
   {
      public PathsViewModel(PathType pathType)
      {
         PathType = pathType;
      }

      public IEnumerable<ProjectUrlPattern> Patterns { get; set; }
      public PathType PathType { get; set; }
   }

   public class UrlsViewModel : ProjectBasedViewModel
   {
      public IEnumerable<ProjectUrl> Urls;
      public bool Irrelevant { get { return Rating == -1; } }
      public bool Hidden { get; set; }
      public int Rating { get; set; }
   }

   public class UrlsOrFilterViewModel : UrlsViewModel
   {
      public IEnumerable<int> Ratings { get; set; }
      public bool? Hdn { get; set; }
      public bool? Ir { get; set; }
   }

   public class CreateOrEditProjectViewModel
   {
      public CreateOrEditProjectViewModel(Project project, IEnumerable<SearchProvider> searchProviders)
      {
         Project = project;
         SearchProviders = searchProviders;
      }

      public Project Project { get; private set; }
      public IEnumerable<SearchProvider> SearchProviders { get; private set; }
   }

}