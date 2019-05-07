document.addEventListener("DOMContentLoaded", function(event) {
    getIp(enviarDados);
});
var clientIp = '';

function enviarDados(ip) {
    clientIp = ip;
    var body = {
        IP: ip,
        URL: window.location.pathname,
        Params: window.location.search,
        Browser: navigator.userAgent
    };
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
        if (this.readyState == 4 && this.status == 200) {

        }
    };
    xhttp.open("POST", "https://localhost:44329/api/access", true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.send(JSON.stringify(body));
}

function getIp(callback) {
    var xhttp = new XMLHttpRequest();
    xhttp.onload = function(json) {
        if (this.readyState == 4 && this.status == 200) {
            var ip = JSON.parse(xhttp.response);
            callback(ip.ip);
        } else {
            callback('Not found');
        }
    };
    xhttp.open("GET", "https://api.ipify.org?format=json&callback=getIP", true);
    xhttp.send();
}