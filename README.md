[Russian](documentation/ru/README.md)
TotalAPI SDK .NET
====================
Introduction
----------------------------------------
TotalAPI is a cloud service providing various API for program complexes development and SDK , which will allow to easily using these API.
TotalAPI provides the developers with user-friendly framework which lowers labor inputs and risks during routine tasks development which are faced within distributed business applications. For instance, management of messages exchange among program systems, access to business objects and organization of program modules interfaces.

Installation
-------------------------------------------
1.  Download [TotalAPI and metriX SDKs](./dist/NET45/)
2.  Download [Linq2Rest](https://www.nuget.org/packages/Linq2Rest/) and [Newtonsoft.Json](https://www.nuget.org/packages/newtonsoft.json/). You could get the packages from Nuget.
3.  Include [Linq2Rest](https://www.nuget.org/packages/Linq2Rest/), [Newtonsoft.Json](https://www.nuget.org/packages/newtonsoft.json/) and  **client** SDK libs into your project.

Getting started
---------------------------------------------
### 1. Register your App
Please, [register](http://billing.totalapi.io/#/applications) your app and you will be provided with the authentication keys ([ApiKey & AppKey](/documentation/auth.md)) and the service URL.

### 2. Tune the SDK with intial settings
Please, define some settings in  the application configuration file `yourapplication.exe.config`.  One should define TotalAPI service URL and authentication key. Please, see about the SDK settings details [here](/documentation/sdksettings.md).
SDK Init settings example:
```xml
    <appSettings>
    	<add key="serverHost" value="http://services.totalapi.io:1202" />
        <add key="apiKey" value="myApiKey" />
    </appSettings>
```
> All initialization data will be provided after registration.

### 3. Include the SDK modules  
Please, include into your project following libs:

* Linq2Rest
* Newtonsoft.Json
* TotalApi.Utils.Common
* TotalApi.Utils.IoC
* TotalApi.Utils.Wcf
* TotalApi.Core
* TotalApi.Billing.Client
* TotalApi.EventManager.Client
* *TotalApi.Telematics.Api*
* *TotalApi.Telematics.Client*

The two last libs (*TotalApi.Telematics.Api* and *TotalApi.Telematics.Client*) are required if you would like to utilise telematic funtionality of the `metrixAPI`. 

> **Attention!** 
> In vast most cases you do not need to include into the project the rest of libs from [here](/dist/NET45). One needs all libraries included if he/she implements a new TotalAPI service only.  Otherwise your app could work improperly.

### 4. Start the SDK in your app
Please, invoke the SDK initialization method when your app starts:
```C#    
    TotalApiBootstrapper.Create();
```
[More detailes concerning assambles loading.](/documentation/sdkload.md)

### 5. Do a simple query to the TotalAPI service
One can test the sevice with following request:
```C#
	// The sample of the using Repository API
	IEnumerable<Application> apps = CoreApi.Repository.ExecuteQuery<Application>();

```
The responce contains registration data.
[Please, see more samples with TotalAPI SDK](/samples/SDK samples/Startup)

Documentation and Samples
-------------------------------------------
* [TotalAPI SDK settings](/documentation/sdksettings.md)
* [Various code samples](/samples/SDK samples)
 * [Telemetric sample application](/documentation/ru/sdksamplestele.md)
 * [Distributed repository](/documentation/ru/sdksamplesrepo.md)
 * [Distributed events](/documentation/ru/sdksamplesevent.md)
 * [Application log](/documentation/ru/sdksampleslog.md)
* [API documentation]()
* [TotalAPI SDK functions description]()
* [User autentification in Total API](/documentation/auth.md)

Known issues
--------------------------------------------------------------
[Please see here](https://github.com/TotalApi/SDK-dNet/issues)

License
------------------------------------------------------------
TotalAPI SDK dstributed with [MIT License](LICENSE.txt)
 
