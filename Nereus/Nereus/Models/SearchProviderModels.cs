using System;
using System.Reflection;
using Nereus.Utils.Search;

namespace Nereus.Models
{
   public class SearchProvider
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string DisplayName { get; set; }
      public string Description { get; set; }
      public string ClassName { get; set; }
      public string ConstructorParam { get; set; }
      public bool SupportsDateConstraints { get; set; }
      public ISearchProvider Instance { get; private set; }

      public ISearchProvider GetInstance()
      {
         InitializeInstance();
         return Instance;
      }

      // Todo: Replace this with MEF or Ninject
      public void InitializeInstance()
      {
         if (Instance != null)
         {
            return;
         }

         var t = Type.GetType(ClassName);
         Instance = (ISearchProvider) (ConstructorParam != null
                                          ? Activator.CreateInstance(t, new object[] {ConstructorParam})
                                          : Activator.CreateInstance(t));
         //var c = t.GetConstructor(new[] { typeof(string) });
         //Instance = (ISearchProvider)c.Invoke(new object[] {ConstructorParam});
      }
   }
}
