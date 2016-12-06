$(function () {
   $("#search-period-datepicker").datepicker({
      "onClose": function () {
         $("#custom-search-period").prop("checked", true);
         SetSearchPeriodLabel();
         $("#search-period-menu").dropdown("hide");
      }
   });

   $("input:radio[name=search-period]").change(function () {
      SetSearchPeriodLabel();
      $("#search-period-menu").dropdown("hide");
   });
   
   $("#search-period-menu li").click(function () {
      $(this).find("input:radio[name=search-period]").prop("checked", true);
   });
});

function SetSearchPeriodLabel() {
   var checkedSearchPeriod = CheckedSearchPeriod();
   var text;
   if (IsCustomPeriod()) {
      text = "Since " + FormatDate($("#search-period-datepicker").datepicker("getDate"));
   } else {
      text = checkedSearchPeriod.siblings("span").text();
   }
   $("#search-period").text(text);
}

function FormatDate(d) {
   return d.getMonth() + "/" + d.getDate() + "/" + d.getFullYear();
}

function CheckedSearchPeriod() {
   return $('input:radio[name=search-period]:checked', '#search-period-menu');
}

function IsCustomPeriod() {
   return CheckedSearchPeriod().attr("id") == "custom-search-period";
}

function getSearchPeriod() {
   var checkedSearchPeriod = CheckedSearchPeriod();

   if (!isUndefined(checkedSearchPeriod)) {
      var searchPeriod;

      if (IsCustomPeriod()) {
         searchPeriod = FormatDate($("#search-period-datepicker").datepicker("getDate"));
      } else {
         searchPeriod = checkedSearchPeriod.parent("li").attr("id").substring(3);
         
         if (searchPeriod == "all") {
            return null;
         }
      }

      return searchPeriod;
   }

   return null;
}

function MillisecondsToDays(milliseconds) {
   return milliseconds / 1000 / 60 / 60 /24;
}