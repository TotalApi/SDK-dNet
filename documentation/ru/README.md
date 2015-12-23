TotalAPI SDK .NET
====================
Введение
----------------------------------------
TotalAPI это Cloud API  решение нового поколения для разработки распределённых масштабируемых модульных программных комплексов. TotalAPI представляет собой облачную службу предоставляющую различные API для разработки программных комплексов, а также SDK  который позволит легко использовать эти API.
TotalAPI предоставляет разработчикам удобный фреймворк, снижающий трудозатраты и риски при разработке рутинных задач, встречающихся в распределенных бизнес приложениях.  Например, организация обмена сообщениями между программными системами, организация доступа к бизнес объектам и интерфейсам программных модулей.   

Инсталляция
-------------------------------------------
1.  Скачайте [TotalAPI и metriX SDKs](../../dist/net45/) 
2.  Скачайте также [Linq2Rest](https://www.nuget.org/packages/Linq2Rest/) и [Newtonsoft.Json](https://www.nuget.org/packages/newtonsoft.json/). Оба пакета доступны через Nuget.
3.  Включите все библиотеки в ваш проект

Как начать использовать SDK
---------------------------------------------
### 1. Регистрация
[Зарегистрируйте](http://welcome.totalapi.io) свое приложение и получите[ ключи ApiKey и AppKey](wiki/auth) для аутентификации ваших запросов а также адрес для подключения к сервису TotalAPI. 

### 2. Инициализация SDK
Выполните инициализацию SDK в файле `yourapplication.exe.config`.  Укажите адрес подключения к сервису TotalAPI и ключи аутентификации. [Подробнее о настройках SDK](sdksettings.md)
```xml
    <appSettings>
    	<add key="serverProtocol" value="https" />
    	<add key="serverHost" value="svc.totalapi.io:4444" />
        <add key="ApiKey" value="myApiKey" />
    </appSettings>
```
> Необходимые значения вы получите при регистрации.

### 3. Подключение модулей SDK  
При старте приложения следует вызвать метод инициализации клиентского SDK:
```C#    
    TotalApiBootstrapper.AppModulesSearchPattern = "myApp.*.dll";
    TotalApiBootstrapper.Create(".");    // Параметром вызова является каталог, в котором находятся файлы SDK. 
                                         // (Или список каталогов, разделённых точкой с запятой).
```
Подробнее о загрузке сборок
### 4. Выполнение запроса к сервису TotalAPI

```C#
	 // The sample of the using Repository API
	 // Getting entities count (without any condition)
	 var startCount = CoreApi.Repository.Count<Device>();
	
	 // Creating a new entity
	 var device = new Device
	 {
        	  Name = "My Device", 
        	  ModelCode = 100,
        	  PhoneNumber = "+555-1111"
     };
      device = CoreApi.Repository.Save(device, true);
    
      // Getting the Id of created entity
      var deviceId = device.Id;
```

Документация и примеры
-------------------------------------------
* [Настройки и инициализация TotalAPI SDK в приложении](sdksettings.md)
* [Примеры использования TotalAPI SDK](../../samples/SDK samples)
* []()
* []()
* []()

Известные проблемы и предложения
--------------------------------------------------------------
ла-ла

Лицензия
------------------------------------------------------------
TotalAPI SDK распостраняется по [лицензии MIT](../../ЛИЦЕНЗИЯ.txt)
 
