$(function () {
   $(".relevance").click(OnItemRelevanceClicked);
   $(".visibility").click(OnItemVisibilityClicked);

   $(".url-item").each(function () {
      var rating = $(this).find("#rating-" + $(this).attr("value"));

      rating.raty({
         score: rating.attr("score"),
         click: function (score)
         {
            OnRatingChanged(score, rating);
            EnableRelevanceFilter();
         }
      });
   });

   $("#actions-dropdown").addClass("anchor-right");

   $(".actions-dropdown-btn").click(function() {
      UrlActionsDropdownShow($(this).parents(".url-item").find(".result-link").attr("href"));
   });

   $("#mark-path-irrelevant-btn").click(function () { ExecutePathAction(MarkPathIrrelevant); });   
   $("#hide-path-btn").click(function () { ExecutePathAction(HidePath); });

   $("#visibility-filter").click(function () {
      if ($(this).hasClass("enabled")) {
         $(this).toggleClass("checked").toggleClass("unchecked");
         window.location = updateSearch(location, "showHidden", $(this).hasClass("checked"));
      }
   });
   
   $("#relevance-filter").click(function () {
      if ($(this).hasClass("enabled")) {
         $(this).toggleClass("checked").toggleClass("unchecked");
         window.location = updateSearch(location, "showIrrelevant", $(this).hasClass("checked"));
      }
   });

   $("#combined-filter").click(function () {
      if ($(this).hasClass("enabled")) {
         $(this).toggleClass("checked").toggleClass("unchecked");
         var isChecked = $(this).hasClass("checked");
         window.location = updateSearch(
            updateSearch(location, "showIrrelevant", isChecked),
            "showHidden",
            isChecked);
      }
   });
});

function HidePath(path) {
   $.post("/api/Project/UrlPattern/Hidden/Create", "=" + path, function () {
      $(".result-link").each(function () {
         if ($(this).attr("href").indexOf(path) != -1) {
            var result = $(this).closest(".url-item");
            var visibility = result.find(".visibility");
            if (visibility.hasClass("not-hidden")) {
               if (!$("#visibility-filter").hasClass("checked")) {
                  result.fadeOut();
               }

               visibility.removeClass("not-hidden").addClass("hidden");

               var numHidden = $("#num-hidden");
               numHidden.text(Number(numHidden.text()) + 1);

               EnableVisibilityFilter();
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
               if (!$("#relevance-filter").hasClass("checked")) {
                  result.fadeOut();
               }

               relevance.removeClass("not-irrelevant").addClass("irrelevant");
               relevance.siblings(".rating").raty("cancel");

               var numIrrelevant = $("#num-irrelevant");
               numIrrelevant.text(Number(numIrrelevant.text()) + 1);

               EnableRelevanceFilter();
            }
         }
      });
   });
}

// Per URL Actions
function OnItemVisibilityClicked() {
   var clicked = $(this);
   var result = clicked.parents(".url-item");
   
   var numHidden = $("#num-hidden");

   if (clicked.hasClass("not-hidden")) {
      if (!$("#visibility-filter").hasClass("checked")) {
         result.fadeOut();
      }

      numHidden.text(Number(numHidden.text()) + 1);

      EnableVisibilityFilter();
   } else {
      numHidden.text(Number(numHidden.text()) - 1);
   }
   
   OnVisibilityClick(clicked);
}

function OnItemRelevanceClicked() {
   var clicked = $(this);
   var result = clicked.closest(".url-item");

   var numIrrelevant = $("#num-irrelevant");

   if (clicked.hasClass("not-irrelevant")) {
      if (!$("#relevance-filter").hasClass("checked")) {
         result.fadeOut();
      }

      numIrrelevant.text(Number(numIrrelevant.text()) + 1);

      EnableRelevanceFilter();
   } else {
      numIrrelevant.text(Number(numIrrelevant.text()) - 1);
   }
   OnRelevanceClick(clicked);
}

// Filter
function EnableRelevanceFilter() {
   $("#relevance-filter").addClass("enabled").removeClass("disabled");
   $("#combined-filter").addClass("enabled").removeClass("disabled");
}

function EnableVisibilityFilter() {
   $("#visibility-filter").addClass("enabled").removeClass("disabled");
   $("#combined-filter").addClass("enabled").removeClass("disabled");
}
