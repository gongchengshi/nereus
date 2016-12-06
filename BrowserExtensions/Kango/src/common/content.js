// ==UserScript==
// @name Content
// @include http://localhost:57603*
// ==/UserScript==

// This works!
var projectId = $.cookie("Project");

window.addEventListener("message", function (evt){
   var newProjectId = evt.data;
   $.get(host + "Project/UserProjectId", function(data) {
      if(newProjectId == data) {
         kango.dispatchMessage('UsingDefaultUserProject', true);
      } else {
         kango.dispatchMessage('UsingDefaultUserProject', false);
      }
   });
});
