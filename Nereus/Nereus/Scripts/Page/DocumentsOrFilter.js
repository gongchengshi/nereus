$(function () {
   $("#visibility").buttonset();
   $("#relevance").buttonset();

   $("input:radio[name=visibility]").change(OnVisibilityFilterChange);
   $("input:radio[name=relevance]").change(OnRelevanceFilterChange);

   $(".rating-filter").click(OnRatingFilterClick);
});

function OnVisibilityFilterChange() {
   var group = $("input:radio[name=visibility]");

   if (group.filter("[id=hidden]").is(":checked")) {
      location.href = updateSearch(location, "hdn", true);
   } else if (group.filter("[id=not-hidden]").is(":checked")) {
      location.href = updateSearch(location, "hdn", false);
   } else {
      location.href = URI(location).removeSearch("hdn");
   }
}

function OnRelevanceFilterChange() {
   var group = $("input:radio[name=relevance]");
   var uri = URI(location);
   var ratingsParam = searchParamValue(uri, "r");

   if (!isUndefined(ratingsParam)) {
      var ratingsList = ratingsParam.split(",");

      if (group.filter("[id=irrelevant]").is(":checked")) {
         if (!("-1" in ratingsList)) {
            ratingsList.push("-1");
            ratingsParam = ratingsList.join(",");
         }

         location.href = updateSearch(uri, "r", ratingsParam);
      } else if (group.filter("[id=not-irrelevant]").is(":checked")) {
         if ("-1" in ratingsList) {
            ratingsList.remove("-1");
            ratingsParam = ratingsList.join(",");
         }
         location.href = updateSearch(uri, "r", ratingsParam);
      } else {
         location.href = URI(location.href).removeSearch("hdn");
      }
   }
}

function OnRatingFilterClick() {

}
