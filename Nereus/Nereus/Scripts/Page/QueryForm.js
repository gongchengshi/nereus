$(function () {
   RestoreSelectFromCookie('Project');
   SaveSelectToCookieOnChange('Project');

   $("select[name=Project]").change(function () {
      var newProjectId = $(this).find(':selected').attr('value');
      SetProjectCookie(newProjectId);
      location.search = "";
   });

   //RestoreSelectFromCookie('SearchProfile');
   //SaveSelectToCookieOnChange('SearchProfile');

   $("input[name=showGooglebotBlockedOnly")[0].checked = URI(location).hasQuery("showGooglebotBlockedOnly", true);

   $('#new-project-search').click(OnNewProjectSearchClick);

   $("#search").click(OnSearchClick);

   $("input[name=q]").keydown(function(event) {
      if (event.keyCode == 13) {
         $("#search").trigger("click");
      }
   });
});

function OnSearchClick() {
   var query = $("input[name=q]").val();
   if (query == "") {
      return;
   }
   
   if ( $('select[name=Project] > option[value=-1]').is(':selected')) {
      ShowNewProjectSearchDialog(query);
      return;
   }

   ExecuteQuery(query);
}

function ExecuteQuery(query) {
   var searchUrl = URI(updateSearch(location, "q", query)).removeSearch("start");

   var searchPeriod = getSearchPeriod();
   if(searchPeriod != null) {
      searchUrl = updateSearch(searchUrl, "searchPeriod", searchPeriod);
   }

   if ($("input[name=showGooglebotBlockedOnly").is(":checked")) {
      searchUrl = updateSearch(searchUrl, "showGooglebotBlockedOnly", true);
   } else {
      searchUrl = URI(searchUrl).removeSearch("showGooglebotBlockedOnly");
   }

   searchUrl = URI(searchUrl).removeSearch("showDuplicates");

   location.href = searchUrl;
}

function OnNewProjectSearchClick() {
   var query = $("input[name=q]").val();
   if (query == "") {
      return;
   }

   ShowNewProjectSearchDialog(query);
}

function ShowNewProjectSearchDialog(query)
{
   $("#dialog-new-project-search").dialog({
      modal: true,
      buttons: {
         "Search": function() {
            $("#dialog-new-project-search-error").hide();
            $("#dialog-default-error").hide();

            var newProjectName = $("#new-project-name").val();
            if (newProjectName == "") {
               return;
            }

            $.ajax({
               url: "/Project/CreateProject",
               type: "POST",
               data: {
                  name: newProjectName,
                  isPrivate: $("#new-project-private").is(":checked")
                   //Todo:Add search provider input etc.
               },
               success: function(data, status, xhr) {
                  if ($.isNumeric(xhr.responseText)) {
                     SetProjectCookie(xhr.responseText);
                     location.search = "q=" + query;
                     $("#dialog-new-project-search").dialog("close");
                  }
               },
               error: function(jqXhr, textStatus, errorThrown) {
                  if (jqXhr.status == 403) {
                     $("#invalid-project-name").text(newProjectName);
                     $("#dialog-new-project-search-error").show();
                  } else {
                     $("#dialog-default-error").text("Error: " + errorThrown + ": " + jqXhr.responseText).show();
                  }
               }
            });
         }
      },
      create: function() {
         $("#dialog-new-project-search").keydown(function(event) {
            if (event.keyCode == 13) {
               $(this).parent()
                  .find("button:eq(1)").trigger("click");
            }
         });

         $("#new-project-name").val($("input[name=q]").val());
      },
      close: function() {
         $("#dialog-new-project-search-error").hide();
         $("#dialog-default-error").hide();
      }
   });
}
