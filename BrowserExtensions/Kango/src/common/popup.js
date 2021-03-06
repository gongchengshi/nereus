var host = "http://localhost:57603/";
var baseUrl = host;
var popupBaseUrl = baseUrl + "Popup/";

KangoAPI.onReady(function() {
   kango.browser.tabs.getCurrent(function (tab) {
      $("#innerFrame").attr("src", popupBaseUrl + "?url=" + tab.getUrl());
   });

   // This is called if the URL changed while the popup is open.
   kango.addMessageListener('URLChanged', function(event){
      $("#innerFrame").attr("src", popupBaseUrl + "?url=" + event.data.currentUrl);
   });

   $("#title").click(function() {
      kango.browser.tabs.create({url:baseUrl});
   });
});
