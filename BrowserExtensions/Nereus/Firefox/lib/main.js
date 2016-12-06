var widgets = require("widget");
var tabs = require("tabs");
var self = require("self");

var mainPopup = require("panel").Panel({
   width:600,
   height:400,
   contentURL: self.data.url("popup.html")
});

var widget = widgets.Widget({
   id: "Nereus",
   label: "Nereus - SEL's Search Platform",
   contentURL: self.data.url("32.png"),
   panel: mainPopup
//   onClick: function() {
//      tabs.open("http://www.selinc.com/");
//   },
//   contentScriptFile: self.data.url("click-listener.js")
});

//widget.port.on("left-click", function(){
//   console.log("left-click");
//});
//
//widget.port.on("right-click", function(){
//   console.log("right-click");
//});

//require("widget").Widget({
//   id: "open-popup-btn",
//   label: "Popup",
//   contentURL: data.url("../Icons/YellowSub2/32.png"),
//   panel: mainPopup
//});
