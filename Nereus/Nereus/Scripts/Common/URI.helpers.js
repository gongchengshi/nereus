function updateSearch(url, key, value) {
   return URI(url).removeSearch(key).addSearch(key, value);
}

function searchParamValue(url, key) {
   return URI.parseQuery(URI(url).search())[key];
}
