**AF - AUTO POPULATE PROPERTIES**
=================================

This is an Umbraco extension that allow developers to configure which document type and/or media type properties must be auto-populate on saving actions.
For each document and/or media types, the developers must specify wich properties must be auto populate and how.
The default configuration file ("afapp.config.json") is stored in "\App_Plugins\AFAutoPopulateProperties\" folder, it contains a basic configuration that is already working.

JSON structure:
- Property "name": the name of AFAPP function (cannot be empty)
- Property "tabs": the sections to which the Aputo Populate Properties functionality will be applied ("Content" e "Media")
- Property "id": section id
- Property "sectionName": section name ("content" or "media")
- Property "label": section tab name
- Property "alias": section alias
- Property "active": if true, set the tab active
- Property "actions": contains the actions list to which the auto-populate functionality will be applied
- Property "action": according to Umbraco directives, the Created Action ("created") has been removed therefore the Auto Populate Properties function now only occurs during the Saving Action ("saving")
- Property "doctypeAlias": if the value is empty the action will be applied to all doctypes otherwise the action will be applied only to the specified doctype
- Property "propertyAlias": Property to auto populate
- Property "propertyType": Property type (actually the value accepted are only string, datetime or bool)
- Property "propertyAliasToCopyValue": The property from which to copy the value (valid only for "saving" action)
- Property "defaultValue": the default value

**_WARNING: This Package override previous version of "afapp.config.json".
Before install this package, be sure to backup this config file and uninstall all previous versions of the package_**

**Latest Changes**
==============
- v8.0.0.0 - Updated package to be compatible with Umbraco 8. According to Umbraco directives, the Creation Action has been removed therefore the Auto Populate Properties function now only occurs during the Saving Action
- v7.3.0.0 - Configuration Tool moved in a custom section
- v7.2.0.0 - Added custom treenode in Developer Section with a smart GUI to create/update the configuration file
- v7.1.0.0 - Added doctype support
- v7.0.0.0 - First release

**Version History**
===============
- v8.0.0.0 - For Umbraco v8.10+ (not tested with previous versions)
- v7.3.0.0 - For Umbraco v7.7+
- v7.1.0.0 - For Umbraco until v7.6.13