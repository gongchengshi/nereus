var host = "http://localhost:57603/"

function CreateJsonRequest() {
   req = new XMLHttpRequest();
   //req.overrideMimeType('application/json');

   return req;
}

function GetLastVisited(url) {
   req = CreateJsonRequest();

   requestUrl = host + "api/Browsing/LastVisited?url=" + url

   req.open("GET", requestUrl, false);
   req.send();

   if (req.status === 200) {
      return req.responseText;
   }
   else if (req.status === 400) {
      return "Never";
   }
   else {
      return req.status;
   }
}
