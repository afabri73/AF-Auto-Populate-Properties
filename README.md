**AF AUTO POPULATE PROPERTIES**
===========================

This is a simple Umbraco extension that allow developers to configure which document type and/or media type properties must be auto-populate on creation and on save actions.
For each document and/or media types, the developers must specify in a configuration file (.json) wich properties must be auto populate and how.
The default configuration file ("AF-AutoPopulateProperties.config.json") is stored in the config folder, it contains an example configuration, it is already working and it can freely modified for the needs of the website. 

With this release you can configure the JSON structure with this params:
- section param: "content" or "media"
- action param: "created" or "saving"
- doctype alias: if the value is empty the action will be applied to all doctypes otherwise the action will be applied only to the specified doctype)
- property alias to auto populate
- property type: actually the value accepted are only string, datetime or bool
- property alias from which to copy the value (valid only for "saving" action)
- default value

In future releases:
- it will create a configuration page to auto-generate the JSON structure

**Latest Changes**
==============
- v7.1.0.0 - Added doctype support
- v7.0.0.0 - First release 