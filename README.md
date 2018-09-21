**AF AUTO POPULATE PROPERTIES**
===========================

This is an Umbraco extension that allow developers to configure which document type and/or media type properties must be auto-populate on creation and on saving actions.
For each document and/or media types, the developers must specify wich properties must be auto populate and how.
The default configuration file ("AF-AutoPopulateProperties.config.json") is stored in the config folder, it contains a basic configuration that is already working.

JSON structure:
- section param: "content" or "media"
- action param: "created" or "saving"
- doctype alias: if the value is empty the action will be applied to all doctypes otherwise the action will be applied only to the specified doctype)
- property alias to auto populate
- property type: actually the value accepted are only string, datetime or bool
- property alias from which to copy the value (valid only for "saving" action)
- default value

**_WARNING: This Package override previous version of "AF-AutoPopulateProperties.config.json". Please be sure to backup it before install this package_**

**_TESTED WITH: Umbraco v7.7.13 - v7.8.3 - v7.9.6 - v7.10.4 - v7.11.1 - v7.12.2_**

**Latest Changes**
==============
- v7.2.0.0 - Added custom treenode in Developer Section with a smart GUI to create/update the configuration file
- v7.1.0.0 - Added doctype support
- v7.0.0.0 - First release

**Version History**
===============
- v7.2.0.0 - For Umbraco v7.7+
- v7.1.0.0 - For Umbraco until v7.6.13
 