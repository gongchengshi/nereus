browserIsChrome = BrowserDetect.browser == "Chrome";
browserIsFirefox = BrowserDetect.browser == "Firefox";

function navigateOnNewTab(targetUrl) {
   if (browserIsChrome) {
      chrome.tabs.create({ url:targetUrl });
   }
}
