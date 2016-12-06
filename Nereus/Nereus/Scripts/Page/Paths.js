$(function () {
   $("#dialog-confirm").hide();
   $(".delete-path").click(OnDeleteClicked);
});

function OnDeleteClicked() {
   var clicked = $(this);
   var result = clicked.parents(".item");
   var resultId = result.attr("id");

   $("#dialog-confirm").dialog({
      resizable: false,
      modal: true,
      buttons: {
         "Delete": function () {
            $.ajax({
               url: "/api/Project/UrlPattern/"+ pathType + "/Delete/" + resultId,
               type: 'DELETE',
               success: function () {
                  result.fadeOut();
               },
               complete: function() {
                  $("#dialog-confirm").dialog("close");
               }
            });
         },
         Cancel: function () {
            $(this).dialog("close");
         }
      }
   });   
}