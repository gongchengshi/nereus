$(function () {;
   var projectId = $.cookie("Project");
   if (projectId != -1) {
      $("input:radio[value=" + projectId + "]").attr("checked", "checked");
   }

   $("input:radio[name=current-project]").change(function () {
      var newProjectId = $(this).attr("value");
      console.log(newProjectId);
      SetProjectCookie(newProjectId);
      // Post a message to content scripts that the project changed.
      window.postMessage(newProjectId, location.origin);
   });

   $("#dialog-delete-confirm").hide();

   $(".delete-project").click(OnDeleteProjectClick);
   
   $("#dialog-private-error").hide();
   $("input[type=checkbox]").attr("disabled", "disabled");
   //$("input.private").change(OnPrivateClick);
});

function OnDeleteProjectClick() {
   var row = $(this).closest("tr");
   var clickedProjectId = row.attr("id");
   var clickedProjectName = row.find("td.project-name").text();
   $("#delete-project-name").text(clickedProjectName);
   $("#dialog-delete-confirm").dialog({
      resizable: false,
      modal: true,
      buttons: {
         "Delete": function() {
            $.ajax({
               url: "/Project/Delete/" + clickedProjectId,
               type: 'DELETE',
               success: function() {
                  row.fadeOut();
               },
               complete: function() {
                  $("#dialog-delete-confirm").dialog("close");
               }
            });
         },
         Cancel: function() {
            $(this).dialog("close");
         }
      }
   });
}
//Checkboxes are disabled through Index and editable through Edit only
//function OnPrivateClick() {
//   var clicked = $(this);
//   $.ajax({
//      url: "/Project/UpdatePrivate",
//      type: "PUT",
//      data: "Id=" + clicked.attr("value") + "&IsPrivate=" + clicked.is(":checked"),
//      statusCode: {
//         403: function() {
//            clicked.removeAttr("checked");
//            $("#dialog-private-error").dialog({
//               resizable: false,
//               modal: true,
//               buttons: {
//                  Ok: function() {
//                     $(this).dialog("close");
//                  }
//               }
//            });
//         }
//      }
//   });
//}