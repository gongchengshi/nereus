// Depends on jquery.cookie.js

$(function() {
   window.postMessage(GetProjectCookie(), location.origin);
});

function SetProjectCookie(value) {
   $.cookie("Project", value, { expires: 365 });
}

function GetProjectCookie() {
   return $.cookie("Project");
}