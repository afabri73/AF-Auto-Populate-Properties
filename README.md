**AF AUTO POPULATE PROPERTIES**
===========================

This is an Umbraco extension that allow developers to configure which document type and/or media type properties must be auto-populate on creation and on saving actions.
For each document and/or media types, the developers must specify wich properties must be auto populate and how.
The default configuration file ("AF-AutoPopulateProperties.config.json") is stored in the config folder, it contains a basic configuration that is already working.

JSON structure:
- Property "section": "content" or "media"
- Property "action": "created" or "saving"
- Property "doctypeAlias": if the value is empty the action will be applied to all doctypes otherwise the action will be applied only to the specified doctype
- Property "propertyAlias": Property to auto populate
- Property "propertyType": Property type (actually the value accepted are only string, datetime or bool)
- Property "propertyAliasToCopyValue": The property from which to copy the value (valid only for "saving" action)
- Property "defaultValue": the default value

**_WARNING: This Package override previous version of "AF-AutoPopulateProperties.config.json".
Before install this package, be sure to backup this config file and uninstall all previous versions of the package_**

**_TESTED WITH: Umbraco v7.7.13 - v7.8.3 - v7.9.6 - v7.10.4 - v7.11.1 - v7.12.2_**

**Latest Changes**
==============
- v7.3.0.0 - Configuration Tool moved in a custom section
- v7.2.0.0 - Added custom treenode in Developer Section with a smart GUI to create/update the configuration file
- v7.1.0.0 - Added doctype support
- v7.0.0.0 - First release

**Version History**
===============
- v7.3.0.0 - For Umbraco v7.7+
- v7.1.0.0 - For Umbraco until v7.6.13