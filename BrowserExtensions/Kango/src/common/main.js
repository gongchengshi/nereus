var ignoredUrls = [
   host,
   "https://www.google.com/search",
   "http://www.google.com/search",
   "http://search.yahoo.com",
   "http://www.bing.com/search"
];

function validUrl(url) {
   if(!url)
   {
      return false;
   }

   for(var i = 0; i < ignoredUrls.length; ++i) {
      if(url.indexOf(ignoredUrls[i]) != -1) {
         return false;
      }
   }

   return true;
}

function Nereus() {
	var self = this;

   kango.ui.browserButton.setPopup({
      url: 'popup.html',
      width: 600,
      height: 400
   });

   kango.browser.addEventListener(kango.browser.event.DOCUMENT_COMPLETE, function (eventDetails){
      if(validUrl(eventDetails.url)) {
         $.get(host + "api/Browsing/Visit?url=" + eventDetails.url); // Notify the server that the page has been visited.
      }
      kango.dispatchMessage('URLChanged', {currentUrl:eventDetails.url});
   });

   kango.addMessageListener('UsingDefaultUserProject', function(event) {
      if(event.data) {
         kango.ui.browserButton.setIcon('icons/32.png');
      } else {
         kango.ui.browserButton.setIcon('icons/32-selected.png');
      }
   });
}

var extension = new Nereus();
