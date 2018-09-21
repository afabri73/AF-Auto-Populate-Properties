angular.module('umbraco.resources')
    .factory('afappResource', function($http) {
        //the factory object returned
        return {
            //this calls the Api Controller we setup earlier
            getJSONConfiguration: function () {
                return $http.get("AFAutoPopulateProperties/ConfigurationApi/GetJSONConfiguration");
            },
            getAllContentTypes: function () {
                return $http.get("AFAutoPopulateProperties/ConfigurationApi/GetAllContentTypes");
            },
            getAllMediaTypes: function () {
                return $http.get("AFAutoPopulateProperties/ConfigurationApi/GetAllMediaTypes");
            },
            getAllProperties: function (section) {
                return $http({
                    method: 'GET',
                    url: 'AFAutoPopulateProperties/ConfigurationApi/GetAllProperties',
                    params: { section: section }
                });
            },
            postGenConf: function (JSONStructure) {
                return $http({
                    method: 'POST',
                    url: 'AFAutoPopulateProperties/ConfigurationApi/PostGenConf',
                    data: JSONStructure
                });
            }
        };
    });