[Russian](ru/sdkload.md)

Using MEF platform
--------------------------------------------------------
TotalAPI SDK uses [MEF](http://bit.do/bJ7CE) platform.
Following code makes composition:
```C#    
    TotalApiBootstrapper.AppModulesSearchPattern = "myApp.*.dll";
    TotalApiBootstrapper.Create();
```
**Good News:**  Now you can use [MEF](http://bit.do/bJ7CE) in your application for your classes without any extra configuration! Please define wildcard mask for assemblies which contain exported classes in the property `AppModulesSearchPattern`. If the property is not defined no assemblies would be loaded.(The assembly which contains initialization of [MEF](http://bit.do/bJ7CE) will be loaded by default).

**Important**
> - Please, do not set mask  \*.dll, in this case it tries to load all assemblies (even system assemblies) what slows down application loading process.
> - Please, do not change SDK file names. All SDK file names masks hard coded as `TotalApi.*.dll`
> - By default program modules are expected in the same folder where the application binary placed or in subfolders.