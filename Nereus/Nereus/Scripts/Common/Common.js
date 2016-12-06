$(function () {
   $('tr').hover(function () {
      $(this).contents('td').css({ 'border': '1px dotted grey', 'border-left': 'none', 'border-right': 'none' });
      $(this).contents('td:first').css('border-left', '1px dotted grey');
      $(this).contents('td:last').css('border-right', '1px dotted grey');
   },
   function () {
      $(this).contents('td').css('border', 'none');
   });

});

function isUndefined(thing) {
   return typeof(thing) === "undefined";
}

Array.prototype.removeAt = function (from, to) {
   var rest = this.slice((to || from) + 1 || this.length);
   this.length = from < 0 ? this.length + from : from;
   return this.push.apply(this, rest);
};

Array.prototype.remove = function(el) {
   return this.splice(this.indexOf(el), 1);
};
