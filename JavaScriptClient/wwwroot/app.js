/// <reference path="oidc-client.js" />

function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}

document.getElementById("login").addEventListener("click", login, false);
document.getElementById("api").addEventListener("click", api, false);
document.getElementById("logout").addEventListener("click", logout, false);
//Servidor de identityserver de prueba
//var config = {
//    authority: "https://demo.identityserver.io",
//    client_id: "implicit.shortlived",
//    redirect_uri: "http://localhost:5003/callback.html",
//    response_type: "id_token token",
//    scope:"openid profile api",
//    post_logout_redirect_uri : "http://localhost:5003/index.html"
//};
//servidor del proyecto
var config = {
    authority: "http://localhost:5000",
    client_id: "js",
    redirect_uri: "http://localhost:5003/callback.html",
    response_type: "id_token token",
    scope: "openid profile Api1",
    post_logout_redirect_uri: "http://localhost:5003/index.html"
};

var mgr = new Oidc.UserManager(config);

mgr.getUser().then(function (user) {
    if (user) {
        log("User logged in", user.profile);
    }
    else {
        log("User not logged in");
    }
});

function login() {
    mgr.signinRedirect();
}

function api() {
    mgr.getUser().then(function (user) {
        var url = "http://localhost:60867/api/test";
        
        var xhr = new XMLHttpRequest();
        xhr.open("GET", url);
        xhr.onload = function () {
           
            log(xhr.status, JSON.parse(xhr.responseText));
        };
        xhr.onerror = function (error) {
            alert("Error", JSON.stringify(error));
        }
        xhr.onreadystatechange = function (oEvent) {
            if (xhr.readyState === 4) {
                if (xhr.status === 200) {
                    console.log(xhr.responseText)
                } else {
                    console.log("Error", JSON.stringify(xhr));
                }
            }
        }; 
        xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        xhr.send();
    });
}

function logout() {
    mgr.signoutRedirect();
}