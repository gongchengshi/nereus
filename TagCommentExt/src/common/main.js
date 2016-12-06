function TagCommentExt() {
	var self = this;
	kango.ui.browserButton.setPopup({
		url: 'popup.html',
		width: 275,
		height: 1000
	});
	kango.ui.browserButton.addEventListener(kango.ui.browserButton.event.COMMAND, function(){
	self._onCommand();
	});
	
	kango.addMessageListener('UsingDefaultUserProject', function(event) {
      if(event.data) {
         kango.ui.browserButton.setIcon('icons/32.png');
      } else {
         kango.ui.browserButton.setIcon('icons/32-selected.png');
      }
   });
}

TagCommentExt.prototype = {

	_onCommand: function() {
		
	}
};

var extension = new TagCommentExt();