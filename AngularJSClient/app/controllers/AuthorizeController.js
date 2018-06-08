(function () {
	'use strict';

	var module = angular.module("mainApp");

	// this code can be used with uglify
	module.controller("AuthorizeController",
		[
            "$log",
            "$scope",
            "$http",
            "SecurityService",
			AuthorizeController
		]
	);

    function AuthorizeController($log, $scope, $http, SecurityService) {
	    $log.info("AuthorizeController called");
		$scope.message = "AuthorizeController created";
	
        SecurityService.DoAuthorization();


            //var deferred = $q.defer();

            //$http({
            //    url: baseUrl + "api/DataEventRecords/" + id.id,
            //    method: "GET"
            //}).success(function (data) {
            //    console.log("GetDataEventRecords success");
            //    deferred.resolve(data);
            //}).error(function (error) {
            //    console.log("GetDataEventRecords error");
            //    deferred.reject(error);
            //});

            //return deferred.promise;

        $http.get("http://localhost:60867/api/test")
                .then(function (response) {
                    alert(JSON.stringify(response));
                    return response.data;
            });

        $http.get("http://localhost:60867/api/Prueba")
            .then(function (response) {
                alert(JSON.stringify(response));
                return response.data;
            });
	}
})();
