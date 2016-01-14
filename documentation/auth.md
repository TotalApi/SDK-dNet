[Russian](/ru/auth.md)

TotalAPI authentication
==========================
Any request to the TotalAPI service requires authentication data. Authentication data represented  by two unique values - secret key *ApiKey* and publick key *AppKey*. The keys are provided for each client application [registered](http://welcome.totalapi.io). One user may register more than one client application. 
TotalAPI authorises data access by authentication data.  Providing a secret ApiKey allows to get access to any data which belongs to the registered application. There is no way to share data between applications using TotalAPI requests.  
Using the ApiKey is most common way to request data from TotalAPI services. Using the ApiKey fits for SaaS applications architecture very good. One uses the ApiKey on the server side to access the TotalAPI data and then authorises data access per role/user depends on business logic implemented on the application backend. 
For light backendless applications one can use AppKey authentication. TotalAPI supports simple authorization based on username/password pair.  Using AppKey allows storing ApiKey in secret.

How to use it
-------------------------------------------------------
TotalAPI authentication requires to define one or more properties of `TotalApiAuth` class and transmitt the data with one of the following ways:
 1. In the Endpoint URL of the service as "auth" parameter in a JSON object of TotalApiAuth:
`https://totalapi.io/api/v1/wcf/ITelematicsService?auth={ApiKey:”456”}`
 1. In the HTTP request header as Authorization field  in a JSON object of TotalApiAuth:
`Authorization: TOTALAPI {ApiKey:”456”}`
 1. In the WCF message header as TotalApiAuth object, for WCF API only.

Authentication types of TotalAPI
-----------------------------------------------------------
**ApiKey- Authentication**
Requires ApiKey. Используется при работе с API в защищенной среде (например, в backend Web-приложения). При успешной аутентификации предоставляется полный доступ ко всем данным приложения, для которого сгенерирован этот ключ.
**AppKey- Authentication**
Requires AppKey. Используется при работе с API в незащищенных клиентских приложениях исключительно для регистрации нового пользователя приложения. При успешной аутентификации разрешены только методы API для задания имени и пароля нового пользователя. Для удаления созданного пользователя требуется ApiKey- аутентификация.
**AppUser- Authentication**
Requires AppKey, UserLogin, UserPassword. Используется при работе с API в незащищенных клиентских приложениях. При успешной аутентификации предоставляется доступ ко всем данным только указанного пользователя.
**SessionKey- Authentication**
Requires SessionKey, полученного предварительным вызовом метода GetSessionKey. Используется при работе с API в незащищенных клиентских приложениях.  При успешной аутентификации уровень доступа к данным такой-же, как и при вызове метода GetSessionKey. Для получения SessionKey необходима ApiKey или AppUser аутентификация.
<!--**SSID- Authentication**
Requires SSID. Используется только при работе подсистем с подсистемой биллинга.
**SuperApiKey- Authentication**
Requires ApiKey суперадминистративного приложения. Используется исключительно при работе с Billing API административным приложением. При успешной аутентификации предоставляется полный доступ ко всем данным биллинга. (Не нужно публично документировать эту возможность).-->

Общие рекомендации по аутентификации TotalAPI
-------------------------------------------------------------------------------
 1. Держите свой ApiKey в секрете. В случае компрометации – получите новый ключ взамен старого.
 1. По возможности, обращайтесь к сервисам API из защищенной среды выполнения вашего приложения. Например, реализуйте все запросы к API на серверной стороне (backend).
 1. Используйте AppKey только в простых приложениях, не имеющих серверной части (backendless).

Класс аутентификации
--------------------------------------------------
[Описание класса TotalApiAuth](http://apiref.totalapi.io), передающего эту информацию.