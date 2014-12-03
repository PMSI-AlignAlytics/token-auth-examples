'use strict';
app.factory('appService', ['$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var appServiceFactory = {};

    var _application = {
        name: "",
        color: ""
    };

    var _get = function () {

        var deferred = $q.defer();
        $http.get(serviceBase + 'api/account/app').success(function (response) {
            _application.name = response.name;
            _application.color =  response.color;
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    }

    appServiceFactory.get = _get;
    appServiceFactory.application = _application;

    return appServiceFactory;
}]);