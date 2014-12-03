'use strict';
app.controller('indexController', ['$scope', '$location', 'authService', 'appService', function ($scope, $location, authService, appService) {

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    $scope.authentication = authService.authentication;
    $scope.application = appService.application;

}]);