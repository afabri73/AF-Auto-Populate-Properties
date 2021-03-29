'use strict';
(function () {
    // Create Edit controller
    function AFAPPJSONConfig($route, $scope, $http, appState, treeService, navigationService, $routeParams, notificationsService, localizationService, eventsService, afappResource) {

        var vm = this;
        vm.JSONStructure = "";
        vm.allContentTypes = "";
        vm.loading = false;

        //Get JSON Configuration
        afappResource.getJSONConfiguration().then(function (response) {
            vm.JSONStructure = response.data[0];
            vm.changeTab(vm.JSONStructure.tabs[0]);
        });

        //Change Tab function
        vm.changeTab = function changeTab(selectedTab) {
            vm.JSONStructure.tabs.forEach(function (tab) {
                tab.active = false;
            });
            selectedTab.active = true;
        };

        //Get all content types via afappResource
        afappResource.getAllContentTypes().then(function (response) {
            vm.allContentTypes = response.data;
        });

        //Get all content properties via afappResource
        afappResource.getAllProperties("content").then(function (response) {
            vm.allContentProperties = response.data;
        });

        //Get all content properties via afappResource with CreateDate and Name properties
        afappResource.getAllProperties("content").then(function (response) {
            vm.allContentPropertiesToCopyValue = response.data;
            vm.allContentPropertiesToCopyValue.unshift("CreateDate");
            vm.allContentPropertiesToCopyValue.unshift("Name");
        });

        //Get all media types via afappResource
        afappResource.getAllMediaTypes().then(function (response) {
            vm.allMediaTypes = response.data;
        });

        //Get all media properties via afappResource
        afappResource.getAllProperties("media").then(function (response) {
            vm.allMediaProperties = response.data;
        });

        //Get all media properties via afappResource with CreateDate and Name properties
        afappResource.getAllProperties("media").then(function (response) {
            vm.allMediaPropertiesToCopyValue = response.data;
            vm.allMediaPropertiesToCopyValue.unshift("CreateDate");
            vm.allMediaPropertiesToCopyValue.unshift("Name");
        });

        //Add new Doctype function
        vm.AddNewDocType = function (sectionAlias, actionAlias) {
            //Get current Section index
            vm.sectionPosition = vm.JSONStructure.tabs.findIndex(s => s.sectionName === sectionAlias);
            //Get current Action index
            vm.actionPosition = vm.JSONStructure.tabs[vm.sectionPosition].actions.findIndex(a => a.action === actionAlias);
            //Add new Doctype in the vm.actionPosition position
            vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes.push({
                "doctypeAlias": "",
                "properties": [{
                    "propertyAlias": "",
                    "config": {
                        "propertyType": "",
                        "propertyAliasToCopyValue": "",
                        "defaultValue": ""
                    }
                }]
            });
        };

        //Remove Doctype function
        vm.RemoveDoctype = function (sectionAlias, actionAlias, doctypeAlias) {
            //Get current Section index
            vm.sectionPosition = vm.JSONStructure.tabs.findIndex(s => s.sectionName === sectionAlias);
            //Get current Action index
            vm.actionPosition = vm.JSONStructure.tabs[vm.sectionPosition].actions.findIndex(a => a.action === actionAlias);
            //Get current Doctype index
            vm.doctypePosition = vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes.findIndex(d => d.doctypeAlias === doctypeAlias);
            //Confirm remove Doctype
            if (confirm("Are you sure to delete this doctype?")) {
                //Remove Doctype
                vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes.splice(vm.doctypePosition, 1);
            }
        };

        //Add new Property into JSONStructure
        vm.AddNewProperty = function (sectionAlias, actionAlias, doctypeAlias) {
            //Get current Section index
            vm.sectionPosition = vm.JSONStructure.tabs.findIndex(s => s.sectionName === sectionAlias);
            //Get current Action index
            vm.actionPosition = vm.JSONStructure.tabs[vm.sectionPosition].actions.findIndex(a => a.action === actionAlias);
            //Get current Doctype index
            vm.doctypePosition = vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes.findIndex(d => d.doctypeAlias === doctypeAlias);
            //Check if a new and empty Property already exist
            if (!vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes[vm.doctypePosition].properties.some(function (o) {
                return o["propertyAlias"] === "";
            })) {
                //Add new property in current doctype
                vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes[vm.doctypePosition].properties.push({
                    "propertyAlias": "",
                    "config": {
                        "propertyType": "",
                        "propertyAliasToCopyValue": "",
                        "defaultValue": ""
                    }
                });
            }
            else {
                //Notify user that a new and empty Property already exist in current Doctype
                alert("Warning! A new and empty Property already exist in '" & vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes[vm.doctypePosition].doctypeAlias & "' doctype. Please configure the existing one before adding a new one.");
            }
        };

        //Remove Property 
        vm.RemoveProperty = function (sectionAlias, actionAlias, doctypeAlias, propertyAlias) {
            //Get current Section index
            vm.sectionPosition = vm.JSONStructure.tabs.findIndex(s => s.sectionName === sectionAlias);
            //Get current Action index
            vm.actionPosition = vm.JSONStructure.tabs[vm.sectionPosition].actions.findIndex(a => a.action === actionAlias);
            //Get current Doctype index
            vm.doctypePosition = vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes.findIndex(d => d.doctypeAlias === doctypeAlias);
            //Get current Property index
            vm.propertyPosition = vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes[vm.doctypePosition].properties.findIndex(p => p.propertyAlias === propertyAlias);
            //Confirm remove Property
            if (confirm("Are you sure to delete this property?")) {
                //Remove Property
                vm.JSONStructure.tabs[vm.sectionPosition].actions[vm.actionPosition].doctypes[vm.doctypePosition].properties.splice(vm.propertyPosition, 1);
            }
        };

        //Configure Update Configuration File - SUCCESS Message
        vm.updateConfigFileSuccess = {
            "type": "success",
            "headline": "",
            "message": "",
            "sticky": false
        };
        localizationService.localize("AFAPP_Config.UpdateJSON.Success.Headline").then(function (value) {
            vm.updateConfigFileSuccess.headline = value;
        });
        localizationService.localize("AFAPP_Config.UpdateJSON.Success.Message").then(function (value) {
            vm.updateConfigFileSuccess.message = value;
        });

        //Configure Update Configuration File - Message
        vm.updateConfigFileError = {
            "type": "error",
            "headline": "",
            "message": "",
            "sticky": false
        };
        localizationService.localize("AFAPP_Config.UpdateConfigFile.ErrorHeadline").then(function (value) {
            vm.updateConfigFileError.headline = value;
        });
        localizationService.localize("AFAPP_Config.UpdateConfigFile.ErrorMessage").then(function (value) {
            vm.updateConfigFileError.message = value;
        });

        //Update Configuration File function
        vm.GenConfPost = function (JSONStructure) {
            afappResource.postGenConf(JSONStructure).then(function (response) {
                if (response.data !== "false") {
                    notificationsService.add(vm.updateConfigFileSuccess);
                }
                else {
                    notificationsService.add(vm.updateConfigFileError);
                }
            });
        };
    }

    //Register the controller
    angular.module("umbraco").controller("AFAPPJSONConfigController", AFAPPJSONConfig);
})();