[English](../../README.md)

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
3.  Включите [Linq2Rest](https://www.nuget.org/packages/Linq2Rest/), [Newtonsoft.Json](https://www.nuget.org/packages/newtonsoft.json/) и **клиентские** библиотеки SDK в ваш проект

Как начать использовать SDK
---------------------------------------------
### 1. Регистрация
[Зарегистрируйте](http://192.168.3.202:4202/#/applications) свое приложение и получите[ ключи ApiKey и AppKey](auth.md) для аутентификации ваших запросов а также адрес для подключения к сервису TotalAPI. 

### 2. Инициализация SDK
Выполните инициализацию SDK в файле `yourapplication.exe.config`.  Укажите адрес подключения к сервису TotalAPI и ключи аутентификации. [Подробнее о настройках SDK](sdksettings.md)
```xml
    <appSettings>
    	<add key="serverProtocol" value="https" />
    	<add key="serverHost" value="svc.totalapi.io:4444" />
        <add key="apiKey" value="myApiKey" />
    </appSettings>
```
> Необходимые значения вы получите при регистрации.

### 3. Подключение модулей SDK  
Для использования `TotalApi SDK` в клиентском приложении необходимо наличие следующих библиотек:

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

Последние две библиотеки необходимы, если вы собираетесь использовать телематическую часть подсистемы `metrixApi`. 

> При написании *клиентского* приложения не включайте в проект другие библиотеки из полного состава. В противном случае возможна некорректная работа приложения.

При старте приложения следует вызвать метод инициализации клиентского SDK:
```C#    
    TotalApiBootstrapper.Create();
```
[Подробнее о загрузке сборок.](sdkload.md)

### 4. Выполнение запроса к сервису TotalAPI
Ниже пример запроса на получение информации о зарегистрированном приложении.
```C#
	// The sample of the using Repository API
	IEnumerable<Application> apps = CoreApi.Repository.ExecuteQuery<Application>();

```
[Пример простейшего приложения с TotalAPI SDK](../../samples/SDK samples/Startup)

Документация и примеры
-------------------------------------------
* [Настройки и инициализация TotalAPI SDK в приложении](sdksettings.md)
* [Примеры использования TotalAPI SDK](../../samples/SDK samples)
 * [Работа с телеметрическими данными](sdksamplestele.md)
 * [Распределенный репозиторий](sdksamplesrepo.md)
 * [Распределенные события](sdksamplesevent.md)
 * [Журналирование работы приложения](sdksampleslog.md)
* [Описание интерфейсов TotalAPI]()
* [Описание функций TotalAPI SDK]()
* [Подробно об аутентификации Total API](auth.md)

Известные проблемы и предложения
--------------------------------------------------------------
[Известные проблемы и предложения](https://github.com/TotalApi/SDK-dNet/issues)

Лицензия
------------------------------------------------------------
TotalAPI SDK распостраняется по [лицензии MIT](../../ЛИЦЕНЗИЯ.txt)
 
