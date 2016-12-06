var lastVisited;
var currentUrl;

var ignoredUrls = [
   host,
   "https://www.google.com/search",
   "http://www.google.com/search",
   "http://search.yahoo.com",
   "http://www.bing.com/search"
];

main();

function main() {

}

function newURL(details) {
   currentUrl = details.url;
   lastVisited = GetLastVisited(details.url);

   if(!validUrl(currentUrl))
   {
      return;
   }

   var req = CreateJsonRequest();
   req.open("GET", host + "api/Browsing/Visit?url=" + details.url, true);
   req.send();
};

// Listen for navigation to a new URL
chrome.webNavigation.onCommitted.addListener(newURL);

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

chrome.tabs.onActivated.addListener( function (activeInfo) {
   chrome.tabs.get(activeInfo.tabId, function (tab) {
      currentUrl = tab.url;
      lastVisited = GetLastVisited(tab.url);
   });
});

