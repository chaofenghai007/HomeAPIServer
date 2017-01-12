//
$.ajax({
    url: 'http://174.36.6.101:8003/api/Trace?formUrl=' + document.referrer,
    type: "GET",
    dataType: 'jsonp',
    jsonp: 'jsoncallback',
    timeout: 2000
});