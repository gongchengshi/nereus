using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gongchengshi;

namespace Nereus.Models
{
   [ComplexType]
   public class SearchPeriod
   {
      public static Dictionary<string, SearchPeriodOption> SearchPeriodOptions { get; private set; }

      static SearchPeriod()
      {
         var searchPeriodOptionValues = EnumExtensions.GetEnumValues<SearchPeriodOption>();
         SearchPeriodOptions = new Dictionary<string, SearchPeriodOption>(searchPeriodOptionValues.Length);
         foreach (var searchPeriod in searchPeriodOptionValues)
         {
            SearchPeriodOptions.Add(searchPeriod.GetAttribute<DisplayAttribute>().ShortName, searchPeriod);
         }
      }

      public SearchPeriod()
      { }

      public SearchPeriod(string searchPeriod)
      {
         if (string.IsNullOrWhiteSpace(searchPeriod))
         {
            return;
         }

         SearchPeriodOption searchPeriodOption;
         DateTime startDate;
         if (SearchPeriodOptions.TryGetValue(searchPeriod, out searchPeriodOption))
         {
            SearchPeriodOption = searchPeriodOption;
         } else if (DateTime.TryParse(searchPeriod, out startDate))
         {
            StartDate = startDate.ToUniversalTime();
         }
         else if (searchPeriod[0] == 'd')
         {
            int days;
            if (int.TryParse(searchPeriod.Substring(1), out days))
            {
               StartDate = DateTime.Now - new TimeSpan(days, 0, 0, 0);
            }
         }
      }

      public SearchPeriod(DateTime? startDate, string searchPeriodOptionShortName) : this(searchPeriodOptionShortName)
      {
         StartDate = startDate;
      }

      public bool IsConstrained { get { return StartDate != null || SearchPeriodOption != SearchPeriodOption.Anytime; } }

      public SearchPeriodOption SearchPeriodOption { get; set; }
      public DateTime? StartDate { get; set; }
   }

   public enum SearchPeriodOption
   {
      [Display(Name = "Any time", ShortName = "all")]
      Anytime,
      [Display(Name = "Past 24 Hours", ShortName = "d1")]
      Past24Hours,
      [Display(Name = "Past 7 Days", ShortName = "d7")]
      PastWeek,
      [Display(Name = "Past 30 Days", ShortName = "d30")]
      Past30Days,
      [Display(Name = "Past Year", ShortName = "y1")]
      PastYear,
      [Display(Name = "Past 2 Years", ShortName = "y2")]
      Past2Years,
      [Display(Name = "Past 5 Years", ShortName = "y5")]
      Past5Years,
      [Display(Name = "Past 10 Years", ShortName = "y10")]
      Past10Years
   }
}
