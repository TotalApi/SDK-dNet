[Russian](./ru/auth.md)

TotalAPI authentication
==========================
Any request to the TotalAPI service requires authentication data. Authentication data represented  by two unique values - secret key *ApiKey* and public key *AppKey*. The keys are provided for each client application [registered](http://billing.totalapi.io). One user may register more than one client application. 
TotalAPI authorizes data access by authentication data.  Providing a secret ApiKey allows to get access to any data which belongs to the registered application. There is no way to share data between applications using TotalAPI requests.  
Using the ApiKey is most common way to request data from TotalAPI services. Using the ApiKey fits for SaaS applications architecture very good. One uses the ApiKey on the server side to access the TotalAPI data and then authorizes data access per role/user depends on business logic implemented on the application back-end. 
For light back-endless applications one can use AppKey authentication. TotalAPI supports simple authorization based on username/password pair.  Using AppKey allows storing ApiKey in secret.

How to use it
-------------------------------------------------------
TotalAPI authentication requires to define one or more properties of [`TotalApiAuth`](http://apiref.totalapi.io) class and transmit the data with one of the following ways:
 1. In the Endpoint URL of the service as "auth" parameter in a JSON object of TotalApiAuth:
`https://totalapi.io/api/v1/wcf/ITelematicsService?auth={ApiKey:”456”}`
 1. In the HTTP request header as Authorization field  in a JSON object of TotalApiAuth:
`Authorization: TOTALAPI {ApiKey:”456”}`
 1. In the WCF message header as TotalApiAuth object, for WCF API only.

Authentication types of TotalAPI
-----------------------------------------------------------
- **ApiKey-Authentication**
Requires ApiKey defined in [`TotalApiAuth`](http://apiref.totalapi.io) class.  This is an essential TotalAPI authentication type. One should use this authentication in a secure environment like server side application. Successful authentication provides full access to any data which belongs to the particular TotalAPI client application.
- **AppKey-Authentication**
Requires AppKey defined in [`TotalApiAuth`](http://apiref.totalapi.io) class. Along with this authentication type only creating a new user allowed. One could use this type of authentication in insecure environments like web client or desktop applications.
- **AppUser-Authentication**
Requires AppKey, UserLogin, UserPassword defined in [`TotalApiAuth`](http://apiref.totalapi.io) class. One could use this type of authentication in insecure environments like web client or desktop applications. Successful authentication provides access to data which belongs to the particular user whose username was provided.
- **SessionKey-Authentication**
Requires SessionKey defined in [`TotalApiAuth`](http://apiref.totalapi.io) class. The session key is provided by GetSessionKey method which should been requested in advance. One could use this type of authentication in insecure environments like web client or desktop applications. Successful authentication provides the same access control which was granted when GetSessionKey method was requested. The SessionKey method requires ApiKey or AppUser authentication.

Resume
-------------------------------------------------------------------------------
1. Keep the ApiKey in secret. In case of compromising of the ApiKey ask for a new one.
1. Use the ApiKey in a secure environment only like server side application.
1. Use the AppKey in simple back-endless applications.

Authentication class
--------------------------------------------------
Please see reference of [TotalApiAuth class](http://apiref.totalapi.io) for more details.