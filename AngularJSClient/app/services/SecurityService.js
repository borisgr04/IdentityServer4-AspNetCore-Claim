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

            mgr.signinRedirect();
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
            mgr.getUser().then(function (user) {
                if (user) {
                    console.log(user);
                    SetAuthorizationData(user.access_token,user.id_token);
                } else {
                    Logoff();
                }
            });
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
