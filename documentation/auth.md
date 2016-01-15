[Russian](./ru/auth.md)

TotalAPI authentication
==========================
Any request to the TotalAPI service requires authentication data. Authentication data represented  by two unique values - secret key *ApiKey* and publick key *AppKey*. The keys are provided for each client application [registered](http://welcome.totalapi.io). One user may register more than one client application. 
TotalAPI authorises data access by authentication data.  Providing a secret ApiKey allows to get access to any data which belongs to the registered application. There is no way to share data between applications using TotalAPI requests.  
Using the ApiKey is most common way to request data from TotalAPI services. Using the ApiKey fits for SaaS applications architecture very good. One uses the ApiKey on the server side to access the TotalAPI data and then authorises data access per role/user depends on business logic implemented on the application backend. 
For light backendless applications one can use AppKey authentication. TotalAPI supports simple authorization based on username/password pair.  Using AppKey allows storing ApiKey in secret.

How to use it
-------------------------------------------------------
TotalAPI authentication requires to define one or more properties of [`TotalApiAuth`](http://apiref.totalapi.io) class and transmitt the data with one of the following ways:
 1. In the Endpoint URL of the service as "auth" parameter in a JSON object of TotalApiAuth:
`https://totalapi.io/api/v1/wcf/ITelematicsService?auth={ApiKey:”456”}`
 1. In the HTTP request header as Authorization field  in a JSON object of TotalApiAuth:
`Authorization: TOTALAPI {ApiKey:”456”}`
 1. In the WCF message header as TotalApiAuth object, for WCF API only.

Authentication types of TotalAPI
-----------------------------------------------------------
- **ApiKey- Authentication**
Requires ApiKey defined in [`TotalApiAuth`](http://apiref.totalapi.io) class.  This is an essential TotalAPI authentication type. One should use this authentication in a secure environment like server side application. Successful authentication provides full access to any data which belongs to the particular TotalAPI client application.
- **AppKey- Authentication**
Requires AppKey defined in [`TotalApiAuth`](http://apiref.totalapi.io) class. Along with this auithentication type only creating a new user allowed. One could use this type of authentication in unsecure environments like web client or desctop applications.
- **AppUser- Authentication**
Requires AppKey, UserLogin, UserPassword defined in [`TotalApiAuth`](http://apiref.totalapi.io) class. One could use this type of authentication in unsecure environments like web client or desctop applications. Successful authentication provides access to data which belongs to the particular user whose username was provided.
- **SessionKey- Authentication**
Requires SessionKey defined in [`TotalApiAuth`](http://apiref.totalapi.io) class. The session key is provided by GetSessionKey method which should been requested in advance. One could use this type of authentication in unsecure environments like web client or desctop applications. Successful authentication provides the same access control which was granted when GetSessionKey method was requested. The SessionKey method requires ApiKey or AppUser authentication.
<!--**SSID- Authentication**
Requires SSID. Используется только при работе подсистем с подсистемой биллинга.
**SuperApiKey- Authentication**
Requires ApiKey суперадминистративного приложения. Используется исключительно при работе с Billing API административным приложением. При успешной аутентификации предоставляется полный доступ ко всем данным биллинга. (Не нужно публично документировать эту возможность).-->
Resume
-------------------------------------------------------------------------------
1. Keep the Api Key in secret. In case of compromising of the ApiKey ask for a new one.
1. Use the ApiKey in a secure environment only like server side application.
1. Use the AppKey in simple backendless applications.

Authentication class
--------------------------------------------------
Please see reference of [TotalApiAuth class](http://apiref.totalapi.io) for more detailes.