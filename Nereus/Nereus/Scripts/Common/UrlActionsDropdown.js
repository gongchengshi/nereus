$(function() {
   $("#custom-path").keydown(function () {
      $("#custom-path-radio").prop("checked", true);
   });
});

// Irrelevant Path Widget
function UrlActionsDropdownShow(url) {
   var options = GetPathOptions(url);

   $(".path-option").remove();
   $("#custom-path-radio").prop("checked", false);

   for (var i = 0; i < options.length - 1; ++i) {
      $("#custom-path-item").before('<li class="path-option"><input type="radio" name="path-option-radio"/><span>' + options[i] + '</span></li>');
   }

   $("#actions-dropdown li").click(function () {
      $(this).find("input:radio").prop("checked", true);
   });

   var customPath = $("#custom-path");
   customPath.attr("value", options[options.length - 1]);
   customPath.attr("orig-path", url);
   customPath.width((url.length + 1) * 6);
}

function GetPathOptions(url) {
   var options = new Array();
   var uri = URI(url);
   var domain = uri.domain();
   var subdomains = uri.subdomain().split(".");

   options[options.length] = domain;

   if (subdomains != "") {
      for (var i = 1; i <= subdomains.length; ++i) {
         options[options.length] = subdomains.slice(i * -1, subdomains.length).join(".") + "." + domain;
      }
   }

   var segments = uri.segment();
   if (segments != "") {
      segments.forEach(function (segment) {
         options[options.length] = options[options.length - 1] + "/" + segment;
      });
   }
   return options;
}

function ExecutePathAction(action) {
   var checked = $('input[name=path-option-radio]:checked', '#actions-dropdown');

   var value;

   var id = checked.attr("id");

   if (id != undefined && id == "custom-path-radio") {
      var customPath = checked.next();
      value = customPath.attr("value");

      var origPath = customPath.attr("orig-path");
      if (origPath.indexOf(value) == -1) {
         var irrelevantDropdownError = $("#actions-dropdown-error");
         irrelevantDropdownError.text("The custom path does not match the selected URL");
         irrelevantDropdownError.show();
         return;
      }
   } else {
      value = checked.next().text();
   }

   $("#actions-dropdown").hide();

   action(value);
}
