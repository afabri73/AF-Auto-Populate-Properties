**AF AUTO POPULATE PROPERTIES**
===========================

This is a simple Umbraco extension that allow developers to configure which document type and/or media type properties must be auto-populate on creation and on save actions.
For each document and/or media types, the developers must specify in a configuration file (.json) wich properties must be auto populate and how.
The default configuration file ("AF-AutoPopulateProperties.config.json") is stored in the config folder, it contains an example configuration, it is already working and it can freely modified for the needs of the website. 

This is the first release and the JSON structure is very simple to configure.
There are:
- a section param: content or media
- an action param: created or saving
- the property alias to auto populate
- the property type: actually the value accepted are only string, datetime or bool
- the property alias from which to copy the value (valid only for "saving" action)
- the default value

In future releases:
- it will be possible group properties by document type. This make it even more flexible and powerful.
- it will create a configuration page to auto-generate the JSON structure

**Latest Changes**
==============
- v7.0.0.0 - First release 