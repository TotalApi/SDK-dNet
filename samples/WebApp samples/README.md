# TotalAPI client Web application sample

Пример работающего приложения - [http://demo.totalapi.io](http://demo.totalapi.io)

Описание
--------
Это пример примитивного приложения отображающего пердвижение транспортных средств, связанных с известными устройствами слежения на карте. 
Ознакомьтесь предварительно с примерами использования TotalApi SDK, в папке [SDK samples](https://github.com/TotalApi/SDK-dNet/tree/master/samples/SDK%20samples), например, [DemoChat](https://github.com/TotalApi/SDK-dNet/tree/master/samples/SDK%20samples/DemoChat).


Начальные шаги
-------------
1. Создадим обычное MVC Web-приложение.
2. Добавим необходимые сборки SDK TotalApi, описанные [здесь](https://github.com/TotalApi/SDK-dNet#3-include-the-sdk-modules).

Настройка `app.config`
----------------------
Укажем протокол и адрес подключения к серверу **TotalApi**.
В данном примере будем использовать уже готовое демонстрационное приложение с `ApiKey` **"DemoApiKey"**. 
```xml
    <appSettings>
        <add key="serverProtocol" value="http" />
        <add key="serverHost" value="services.totalapi.io:1202" />
        <add key="apiKey" value="DemoApiKey" />
    </appSettings>
```

Инициализация SDK TotalApi
--------------------------
Первой строчкой нашего приложения должна быть:
```C#
TotalApiBootstrapper.Create();
```
При создании, бутстраппер загружает все необходимые сборки, выполняет настройку MEF-композиции, необходимой для работы приложения. Связывается с сервером TotalApi и получает необходимую информацию о подсистемах программного комплекса.   