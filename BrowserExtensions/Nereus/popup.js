function onContentLoaded(){
   $("title").addEventListener('click', function () {
      navigateOnNewTab('http://localhost:57603/Popup')
   });

   //$("LastVisitedValue").innerHTML = chrome.extension.getBackgroundPage().lastVisited;

   $("innerFrame").setAttribute("src", "http://localhost:57603/Popup?url=" + chrome.extension.getBackgroundPage().currentUrl);
}

document.addEventListener('DOMContentLoaded', onContentLoaded);

