'use strict';
(function () {
    // Create Edit controller
    function AFAPPDashboardController($route, $scope, $http, appState, treeService, navigationService, $routeParams, afappResource, notificationsService, localizationService, eventsService) {
        // Set a property on the scope equal to the current route id
        $scope.id = $routeParams.id;
    }

    // Register the controller
    angular.module("umbraco").controller("AFAPPDashboardController", AFAPPDashboardController);
})();