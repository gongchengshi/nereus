function SaveSelectToCookieOnChange(name) {
   var select = $('select[name=' + name + ']');

   select.change(function () {
      var value = select.find(':selected').attr('value');
      if (value != "-1") {
         $.cookie(name, value, { expires: 365 });
      }
   });
}

function RestoreSelectFromCookie(name) {
   var value = $.cookie(name);

   if (value) {
      $('select[name=' + name + '] > option[value=' + value + ']').prop('selected', true);
   } else {
      $('select[name=' + name + '] > option[value=-1]').prop('selected', true);
   }
}