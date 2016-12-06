function insertParam(searchPart, key, value) {
   key = escape(key); value = escape(value);

   var kvp = searchPart.split('&');
   if (kvp == '') {
      searchPart = '?' + key + '=' + value;
   }
   else {
      var i = kvp.length; var x; while (i--) {
         x = kvp[i].split('=');

         if (x[0] == key) {
            x[1] = value;
            kvp[i] = x.join('=');
            break;
         }
      }

      if (i < 0) { kvp[kvp.length] = [key, value].join('='); }

      searchPart = kvp.join('&');
   }

   return searchPart;
}
