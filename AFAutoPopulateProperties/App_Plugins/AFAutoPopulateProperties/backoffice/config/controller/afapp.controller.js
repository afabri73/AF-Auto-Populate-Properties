'use strict';
(function () {
    // Create Edit controller
    function AFAPPJSONConfig($route, $scope, $http, $routeParams, afappResource, notificationsService, localizationService, eventsService) {
        // Set a property on the scope equal to the current route id
        $scope.id = $routeParams.id;

        // Reload page function
        $scope.reloadRoute = function () {
            $route.reload();
        };

        // Event to mark the current treenode as selected 
        eventsService.on("appState.treeState.changed", function (event, args) {
            if (args.key === "selectedNode") {

                event.currentScope.nav.syncTree({
                    tree: $routeParams.tree,
                    path: buildPath(args.value, []),
                    forceReload: false
                });
            }
        });

        // Get all content types via afappResource
        afappResource.getAllContentTypes().then(function (response) {
            $scope.allContentTypes = response.data;
        });

        // Get all content properties via afappResource
        afappResource.getAllProperties("content").then(function (response) {
            $scope.allContentProperties = response.data;
        });

        // Get all content properties via afappResource with CreateDate and Name properties
        afappResource.getAllProperties("content").then(function (response) {
            $scope.allContentPropertiesToCopyValue = response.data;
            $scope.allContentPropertiesToCopyValue.unshift("CreateDate");
            $scope.allContentPropertiesToCopyValue.unshift("Name");
        });

        // Get all media types via afappResource
        afappResource.getAllMediaTypes().then(function (response) {
            $scope.allMediaTypes = response.data;
        });

        // Get all media properties via afappResource
        afappResource.getAllProperties("media").then(function (response) {
            $scope.allMediaProperties = response.data;
        });

        // Get all media properties via afappResource with CreateDate and Name properties
        afappResource.getAllProperties("media").then(function (response) {
            $scope.allMediaPropertiesToCopyValue = response.data;
            $scope.allMediaPropertiesToCopyValue.unshift("CreateDate");
            $scope.allMediaPropertiesToCopyValue.unshift("Name");
        });

        // Get JSON Configuration
        afappResource.getJSONConfiguration().then(function (response) {
            $scope.JSONStructure = response.data;
        });

        // Add new and empty Doctype
        $scope.AddNewDocType = function (sectionAlias, actionAlias, ) {
            $scope.sectionPosition = $scope.JSONStructure.findIndex(s => s.section === sectionAlias);
            $scope.actionPosition = $scope.JSONStructure[$scope.sectionPosition].actions.findIndex(a => a.action === actionAlias);
            // Add new property into doctype
            $scope.JSONStructure[$scope.sectionPosition].actions[$scope.actionPosition].doctypes.push({ "doctypeAlias": "", "properties": [{ "propertyAlias": "", "config": { "propertyType": "", "propertyAliasToCopyValue": "", "defaultValue": "" } }] });
        };

        // Remove new and empty Doctype
        $scope.RemoveDoctype = function (sectionAlias, actionAlias, doctypeAlias) {
            $scope.sectionPosition = $scope.JSONStructure.findIndex(s => s.section === sectionAlias);
            $scope.actionPosition = $scope.JSONStructure[$scope.sectionPosition].actions.findIndex(a => a.action === actionAlias);
            $scope.doctypePosition = $scope.JSONStructure[$scope.sectionPosition].actions[$scope.actionPosition].doctypes.findIndex(d => d.doctypeAlias === doctypeAlias);
            if (confirm("Are you sure to delete this doctype?")) {
                $scope.JSONStructure[$scope.sectionPosition].actions[$scope.actionPosition].doctypes.splice($scope.doctypePosition, 1);
            }
        };

        // Add new and empty Property into JSONStructure
        $scope.AddNewProperty = function (sectionAlias, actionAlias, doctypeAlias) {
            $scope.sectionPosition = $scope.JSONStructure.findIndex(s => s.section === sectionAlias);
            $scope.actionPosition = $scope.JSONStructure[$scope.sectionPosition].actions.findIndex(a => a.action === actionAlias);
            $scope.doctypePosition = $scope.JSONStructure[$scope.sectionPosition].actions[$scope.actionPosition].doctypes.findIndex(d => d.doctypeAlias === doctypeAlias);
            // Check if a new and empty Property already exist
            if (!$scope.JSONStructure[$scope.sectionPosition].actions[$scope.actionPosition].doctypes[$scope.doctypePosition].properties.some(function (o) { return o["propertyAlias"] === ""; })) {
                // Add new property into doctype
                $scope.JSONStructure[$scope.sectionPosition].actions[$scope.actionPosition].doctypes[$scope.doctypePosition].properties.push({ "propertyAlias": "", "config": { "propertyType": "", "propertyAliasToCopyValue": "", "defaultValue": "" } });
            }
            else {
                // Notify user that a new and empty Property already exist
                alert("Warning! A new and empty Property already exist. Please configure the existing one before adding a new one.");
            }
        };

        // Remove Property 
        $scope.RemoveProperty = function (sectionAlias, actionAlias, doctypeAlias, propertyAlias) {
            $scope.sectionPosition = $scope.JSONStructure.findIndex(s => s.section === sectionAlias);
            $scope.actionPosition = $scope.JSONStructure[$scope.sectionPosition].actions.findIndex(a => a.action === actionAlias);
            $scope.doctypePosition = $scope.JSONStructure[$scope.sectionPosition].actions[$scope.actionPosition].doctypes.findIndex(d => d.doctypeAlias === doctypeAlias);
            $scope.propertyPosition = $scope.JSONStructure[$scope.sectionPosition].actions[$scope.actionPosition].doctypes[$scope.doctypePosition].properties.findIndex(p => p.propertyAlias === propertyAlias);
            if (confirm("Are you sure to delete this property?")) {
                $scope.JSONStructure[$scope.sectionPosition].actions[$scope.actionPosition].doctypes[$scope.doctypePosition].properties.splice($scope.propertyPosition, 1);
            }
        };

        // Regenerate and Update Configuration File (POST)
        $scope.updateConfigFileSuccess = {
            "type": "success",
            "headline": "",
            "sticky": false
        };
        localizationService.localize("AFAPP_Config.UpdateConfigFile.SuccessHeadline").then(function (value) {
            $scope.updateConfigFileSuccess.headline = value;
        });
        localizationService.localize("AFAPP_Config.UpdateConfigFile.SuccessMessage").then(function (value) {
            $scope.updateConfigFileSuccess.message = value;
        });

        $scope.updateConfigFileError = {
            "type": "error",
            "headline": "",
            "sticky": false
        };
        localizationService.localize("AFAPP_Config.UpdateConfigFile.ErrorHeadline").then(function (value) {
            $scope.updateConfigFileError.headline = value;
        });
        localizationService.localize("AFAPP_Config.UpdateConfigFile.ErrorMessage").then(function (value) {
            $scope.updateConfigFileError.message = value;
        });

        $scope.genConfPost = function (JSONStructure) {
            afappResource.postGenConf(JSONStructure).then(function (response) {
                if (response.data !== "false") {
                    notificationsService.add($scope.updateConfigFileSuccess);
                }
                else {
                    notificationsService.add($scope.updateConfigFileError);
                }
            });
        };
    }

    // Method to get the node path (used for event to mark the current treenode as selected)
    function buildPath(node, path) {
        path.push(node.id);
        if (node.id === "-1") return path.reverse();

        var parent = node.parent();
        if (parent === undefined) return path;

        return buildPath(parent, path);
    }

    // Register the controller
    angular.module("umbraco").controller("AFAPPJSONConfigController", AFAPPJSONConfig);
})();