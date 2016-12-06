$(function () {
   $.fn.raty.defaults.path = "/Images/Raty/";
   $.fn.raty.defaults.cancel = true;
   $.fn.raty.defaults.number = 3;
   $.fn.raty.defaults.hints = ["Rating", "Rating", "Rating"];
   $.fn.raty.defaults.cancelHint = "Clear rating";
});

function OnRatingChanged(score, ratingElement) {
   var relevance = ratingElement.closest(".url-item").find(".relevance");

   var itemId = ratingElement.attr("id").substring(7);
   $.ajax({
      url: "/api/ProjectUrl/Rate/" + itemId,
      type: 'PUT',
      data: "=" + score,
      success: function () {
         relevance.removeClass("irrelevant").addClass("not-irrelevant");
      }
   });
}

function OnRelevanceClick(clicked) {
   var itemId = clicked.closest(".url-item").find(".rating").attr("id").substring("7");
   
   if (clicked.hasClass("not-irrelevant")) {
      $.ajax({
         url: "/api/ProjectUrl/Rate/" + itemId,
         type: 'PUT',
         data: "=-1",
         success: function () {
            clicked.removeClass("not-irrelevant").addClass("irrelevant");
            clicked.closest(".url-item").find(".rating").raty("cancel");
         }
      });
   } else if (clicked.hasClass("irrelevant")) {
      $.ajax({
         url: "/api/ProjectUrl/Rate/" + itemId,
         type: 'PUT',
         data: "=0",
         success: function() {
            clicked.removeClass("irrelevant").addClass("not-irrelevant");
         }
      });
   }
}

function OnVisibilityClick(clicked) {
   var itemId = clicked.closest(".url-item").find(".rating").attr("id").substring("7");

   if (clicked.hasClass("hidden")) {
      $.ajax({
         url: "/api/ProjectUrl/Show/" + itemId,
         type: 'PUT',
         success: function () {
            clicked.addClass("not-hidden").removeClass("hidden");
         }
      });
   } else if (clicked.hasClass("not-hidden")) {
      $.ajax({
         url: "/api/ProjectUrl/Hide/" + itemId,
         type: 'PUT',
         success: function () {
            clicked.addClass("hidden").removeClass("not-hidden");
         }
      });
   }
}
