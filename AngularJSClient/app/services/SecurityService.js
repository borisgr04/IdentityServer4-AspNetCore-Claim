(function () {
    'use strict';

    function SecurityService($http, $log, $q, $rootScope,  $window, $state, localStorageService) {
        $log.info("SecurityService called");

        var config = {
            authority: "http://localhost:5000",
            client_id: "jsAngular",
            redirect_uri: "https://localhost:44376/authorized",
            response_type: "id_token token",
            scope: "openid profile Api1",
            post_logout_redirect_uri: "https://localhost:44376/unauthorized"
        };

        var mgr = new Oidc.UserManager(config);

        $rootScope.IsAuthorized = false;
        $rootScope.HasAdminRole = false;

        function urlBase64Decode(str) {
            var output = str.replace('-', '+').replace('_', '/');
            switch (output.length % 4) {
                case 0:
                    break;
                case 2:
                    output += '==';
                    break;
                case 3:
                    output += '=';
                    break;
                default:
                    throw 'Illegal base64url string!';
            }
            return window.atob(output);
        }

        function getDataFromToken(token) {
            var data = {};
            if (typeof token !== 'undefined') {
                var encoded = token.split('.')[1];
                data = JSON.parse(urlBase64Decode(encoded));
            }
            return data;
        }

        var ResetAuthorizationData = function () {
            localStorageService.set("authorizationData", "");
            localStorageService.set("authorizationDataIdToken", "");
            $rootScope.IsAuthorized = false;
            $rootScope.HasAdminRole = false;
        }

        var SetAuthorizationData = function (token, id_token) {
            
            if (localStorageService.get("authorizationData") !== "") {
                localStorageService.set("authorizationData", "");
            }

            localStorageService.set("authorizationData", token);
            localStorageService.set("authorizationDataIdToken", id_token);
            $rootScope.IsAuthorized = true;

            var data = getDataFromToken(token);
            for (var i = 0; i < data.role.length; i++) {
                if (data.role[i] === "dataEventRecords.admin") {
                    $rootScope.HasAdminRole = true;                    
                }
            }
        }

        var authorize = function () {
            console.log("AuthorizedController time to log on");

            //GET /authorize?
            //response_type=code%20id_token
            //&client_id=s6BhdRkqt3
            //&redirect_uri=https%3A%2F%2Fclient.example.org%2Fcb
            //&scope=openid%20profile%data
            //&nonce=n-0S6_WzA2Mj
            //&state=af0ifjsldkj HTTP/1.1


            //ASI ESTABA ANTES
            //var authorizationUrl = 'https://localhost:44318/connect/authorize';
            //var client_id = 'angularjsclient';
            //var redirect_uri = 'https://localhost:44376/authorized';
            //var response_type = "id_token token";
            //var scope = "dataEventRecords securedFiles openid";
            //var nonce = "N" + Math.random() + "" + Date.now();
            //var state = Date.now() + "" + Math.random();

            /* TOMADO DEL EJEMPLO DE javascript
             * var config = {
    authority: "http://localhost:5000",
    client_id: "js",
    redirect_uri: "http://localhost:5003/callback.html",
    response_type: "id_token token",
    scope: "openid profile Api1",
    post_logout_redirect_uri: "http://localhost:5003/index.html"
};
             **/
            alert("version 3");
            var authorizationUrl = 'http://localhost:5000/Account/Login';
            var client_id = 'jsAngular';
            var redirect_uri = 'https://localhost:44376/authorized';
            var response_type = "id_token token";
            var scope = "openid profile Api1";
            var nonce = "N" + Math.random() + "" + Date.now();
            var state = Date.now() + "" + Math.random();

            localStorageService.set("authNonce", nonce);
            localStorageService.set("authStateControl", state);
            console.log("AuthorizedController created. adding myautostate: " + localStorageService.get("authStateControl"));

            var url =
                authorizationUrl + "?" +
                "returnUrl=" + encodeURI("/connect/authorize/callback")+"?"+
                "response_type=" + encodeURI(response_type) + "&" +
                "client_id=" + encodeURI(client_id) + "&" +
                "redirect_uri=" + encodeURI(redirect_uri) + "&" +
                "scope=" + encodeURI(scope) + "&" +
                "nonce=" + encodeURI(nonce) + "&" +
                "state=" + encodeURI(state);
            //http://localhost:5000/account/login?returnUrl=/connect/authorize/callback?client_id=js&redirect_uri=http%3A%2F%2Flocalhost%3A5003%2Fcallback.html&response_type=id_token%20token&scope=openid%20profile%20Api1&state=3a8d0adbeac64b6686848916ba1b082a&nonce=4ee3d81ea047455cbc57220246b27569
            //http://localhost:5000/Account/Login?response_type=id_token token&client_id=jsAngular&redirect_uri=https://localhost:44376/authorized&scope=openid profile Api1&nonce=N0.28977797428450171528383002719&state=15283830027190.5695996344992398
            //$window.location = url;

            
            
            mgr.signinRedirect();

        }

        var authorizeCallback = function () {
            console.log("AuthorizedController created, has hash");
            var hash = window.location.hash.substr(1);

            var result = hash.split('&').reduce(function (result, item) {
                var parts = item.split('=');
                result[parts[0]] = parts[1];
                return result;
            }, {});

            var token = "";
            var id_token = "";
            var authResponseIsValid = false;
            if (!result.error) {
                
                    if (result.state !== localStorageService.get("authStateControl")) {
                        console.log("AuthorizedCallback incorrect state");
                    } else {

                        token = result.access_token;
                        id_token = result.id_token

                        var dataIdToken = getDataFromToken(id_token);
                        console.log(dataIdToken);

                        // validate nonce
                        if (dataIdToken.nonce !== localStorageService.get("authNonce")) {
                            console.log("AuthorizedCallback incorrect nonce");
                        } else {
                            localStorageService.set("authNonce", "");
                            localStorageService.set("authStateControl", "");

                            authResponseIsValid = true;
                            console.log("AuthorizedCallback state and nonce validated, returning access token");
                        }
                    }    
            }

            if (authResponseIsValid) {
                SetAuthorizationData(token, id_token);
                console.log(localStorageService.get("authorizationData"));

                $state.go("overviewindex");
            }
            else {
                ResetAuthorizationData();
                $state.go("unauthorized");
            }
            
        }

        var IsAuthorized = function () {
            
            console.log(mgr);
            mgr.getUser().then(function (user,xx) {
                alert(JSON.stringify(user));
                alert(JSON.stringify(xx));
                if (user) {
                    authorizeCallback();
                }
            });
        }

        var DoAuthorization = function () {
            //new Oidc.UserManager().signinRedirectCallback().then(function (user) {
            //    alert("Si esta logueado" + JSON.stringify(user));
            //}).catch(function (e) {
            //    alert("Error");
            //    console.error(e);
            //    });


            //console.log(mgr);
            mgr.getUser().then(function (user) {
                if (user) {
                    alert("Si esta logueado" + JSON.stringify(user));
                    authorizeCallback();
                } else {
                    alert("NO esta logueado" + JSON.stringify(user));
                }
            });
            ResetAuthorizationData();

            if ($window.location.hash) {
                authorizeCallback();
            }
            else {
                authorize();
            }
        }

        var Logoff = function () {
            ResetAuthorizationData();
            $window.location = "https://localhost:44346/unauthorized.html";
        }

        return {
            ResetAuthorizationData: ResetAuthorizationData,
            SetAuthorizationData: SetAuthorizationData,
            DoAuthorization: DoAuthorization,
            IsAuthorized: IsAuthorized,
            Logoff: Logoff
        }
    }

    var module = angular.module('mainApp');

    module.factory("SecurityService",
        [
            "$http",
            "$log",
            "$q",
            "$rootScope",
            "$window",
            "$state",
            "localStorageService",
            SecurityService
        ]
    );

})();
