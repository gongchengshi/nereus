$(function () {
   $.fn.raty.defaults.path = "/Images/Raty/";
   $.fn.raty.defaults.cancel = true;
   $.fn.raty.defaults.number = 3;

   $(".url-item").each(function() {
      var rating = $(this).find("#rating-"+($(this).attr("id")).substring(5));

      rating.raty({
         score: rating.attr("score"),
         click: function (score) { OnRatingChanged(score, rating); }
      });
   });

   var ratingFilter = $("#rating-filter");

   ratingFilter.raty({
      score: ratingFilter.attr("score"),
      click: function (score) {
         location.href = updateSearch(location, "r", score == null? 0 : score);
      }
   });

   $(".relevance").click(function() { OnRelevanceClick($(this)); });
   $(".visibility").click(function () { OnVisibilityClick($(this)); });

   $("#relevance-filter").click(function() {
      var rating = $(this).hasClass("irrelevant") ? 0 : -1;
      location.href = updateSearch(location, "r", rating);
   });

   $("#visibility-filter").click(function() {
      location.href = updateSearch(location, "hdn", $(this).hasClass("not-hidden"));
   });

   $(".actions-dropdown-btn").click(function () {
      UrlActionsDropdownShow($(this).parents(".url-item").find(".result-link").attr("href"));
   });

   $("#mark-path-irrelevant-btn").click(function () { ExecutePathAction(MarkPathIrrelevant); });
   $("#hide-path-btn").click(function () { ExecutePathAction(HidePath); });
});

function HidePath(path) {
   $.post("/api/Project/UrlPattern/Hidden/Create", "=" + path, function () {
      $(".result-link").each(function () {
         if ($(this).attr("href").indexOf(path) != -1) {
            var result = $(this).closest(".url-item");
            var visibility = result.find(".visibility");
            if (visibility.hasClass("not-hidden")) {
               visibility.removeClass("not-hidden").addClass("hidden");
               
               if (!$("#visibility-filter").hasClass("hidden")) {
                  result.fadeOut();
               }
            }
         }
      });
   });
}

function MarkPathIrrelevant(path) {
   $.post("/api/Project/UrlPattern/Irrelevant/Create", "=" + path, function () {
      $(".result-link").each(function () {
         if ($(this).attr("href").indexOf(path) != -1) {
            var result = $(this).closest(".url-item");
            var relevance = result.find(".relevance");
            if (relevance.hasClass("not-irrelevant")) {
               relevance.removeClass("not-irrelevant").addClass("irrelevant");
               
               if (!$("#relevance-filter").hasClass("irrelevant")) {
                  result.fadeOut();
               }
            }
         }
      });
   });
}
