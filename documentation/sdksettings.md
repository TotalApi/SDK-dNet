[Russian](/ru/sdksettings.md)

TotalAPI SDK Settings
================================================================
All SDK settings defined in section  `<appSettings>` of configuration file `app.exe.config` of user application.
SDK Initialization settings
---------------------------------------------------------------------
| Parameter		|Description|
|------------------------------|---------------|
| serverHost	       		|Address (IP or domain name) of the service host and network port. If network port ommited a standard port is used depends on the network protocol defined in patameter *serverProtocol*. **By default  -  localhost.**|
| serverProtocol		|Wide known [IANA Internet service name](http://www.iana.org/assignments/service-names-port-numbers/service-names-port-numbers.xhtml) which defines Transport Protocol Port Number, for instance:  parameter value "http" means port 80. This Port Number is substituted into TotalAPI service host address.  **By default  -  https** (port 443, using https requires SSL sertificate has been installed).|
| apiKey			|The ApiKey value. If this parameter ommited one can define propertie  TotalApiAuth.ApiKey before MefBootstrapper started.|
| appKey			|The AppKey value. If this parameter ommited one can define propertie  TotalApiAuth.AppKey before MefBootstrapper started. If both AppKey and ApiKey are defined the value of AppKey is ignored.|
|isDebugMode 		| Debug mode flag. Applicapable for a WebApp only. If the value is **true** - Debug mode|
|modulesFolders	|List of folders where the binaries of program modules are placed. The values in the list are separated with semicolon (;). It supports relative path from the application binary file as well as recursion of subfolders. All binaries from the application folder (.) are loaded by default.  The mask **TotalApi.*.dll**. is applied for all binaries names.  **By default  -  Modules.**|
|logThreadEnabled	|Flag of the application log queue. True value means all log messages are sent to the queue, false value means no queue is used what could slow down the application.  **By default  -  true**|

Appllication log parameters
--------------------------------------------------------------------------------------------------------
The following settings control data output with interface ILogger. By default your application uses class FileLogger as log worker by default.

| Parameter		|Description|
|------------------------------------|---------------|
|totalapi.log.file 		| Log file name. It supports relative and full path as well as macroses of FileSystemExtension.ExpandPath. If parameter value is CON it means print the log out to the console instead of a file.|
|totalapi.log.console 	| Flag of printing the log out to the console.  If true - to print out the log to console a long with a file, false - log into a file only. **By default  -  false**, in DEBUG build value by default is true.|
|totalapi.log.level 	        | Bit mask which defines logging level: x01 - information messages, x02 - warnings, x04 - errors. **By default  - x07**|
|totalapi.log.append 	| Flag which controls whether to create new log file or append existing one when the application starts. True -create new one, False - append existing file.  **By default  -  false**|

Example 
---------------------------------------------------------------
Initializing TotalAPI SDK into app.config:  
```xml
    <appSettings>
        <add key="serverProtocol" value="https" />
    	<add key="serverHost" value="svc.totalapi.io:4444" />
        <add key="ApiKey" value="myApiKey" />
        <add key="modulesFolders" value="D:\myModulesFolder" />
        <add key="totalapi.log.file" value="myErrorMsg.txt"/>
        <add key="totalapi.log.level" value="4"/>
        <add key="totalapi.log.console" value="true"/>
    </appSettings>
```
Extended log parameters
----------------------------------------------------------------------------------
Extended logging allows to define more than one log file. 
One can add section  totalapi.log:
```xml
	  <configSections>
		  <section name="totalapi.log" type="TotalApi.Core.Api.FileLoggerConfiguration, TotalApi.Core" />
	  </configSections>  
```
Than in section files of section totalapi.log  one can define two log files:
```xml
	  <totalapi.log>
              <files>
                  <add file="con" level="7" />
                  <add file="c:\temp\log.txt" console="true" append="true" />
             </files>
      </totalapi.log>
```
**Attention!**
> Please, do not place log file in the same folder/subfolder with Web App binaries. In this case Web application restarts every time when log file is updated.

