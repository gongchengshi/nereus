$(function() {
   $(".delete-query").click(OnDeleteQueryClick);
});

function OnDeleteQueryClick() {
   var row = $(this).closest("tr");
   var clickedQueryId = row.attr("id");

   $.ajax({
      url: "/api/Queries/Delete/" + clickedQueryId,
      type: 'DELETE',
      success: function() {
         row.fadeOut();
      }
   });
}
