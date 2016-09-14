Demo Chat
=========

Описание
--------
Это пример создания примитивного чата между пользователями одного программного комплекса. Пример показывает сразу несколько технологий  построения распределённого приложения на базе SDK TotalApi.

Начальные шаги
-------------
1. Создадим обычное консольное приложение.
2. Добавим необходимые сборки SDK TotalApi, описанные [здесь](https://github.com/TotalApi/SDK-dNet#3-include-the-sdk-modules).

Настройка `app.config`
----------------------
Укажем протокол и адрес подключения к серверу **TotalApi**.  
После [регистрации](http://billing.totalapi.io/#/register/free) своего приложения вы получите пару ключей `ApiKey` и `AppKey`, а также адрес подключения к серверу **TotalApi**.
Параметры подключения **должны** отличаться от приведенных в примере. Вы должны подставить значения, которые вы получите после регистрации своего приложения.
```xml
    <appSettings>
        <add key="serverHost" value="http://services.totalapi.io:1202" />
        <add key="appKey" value="07088143-08e7-416f-8c33-01e80f59ccae" />
    </appSettings>
```
Обратите внимание, что в `app.config` мы указываем `AppKey`, а не `ApiKey`, так как наше чат-приложение будет свободно распространяться потенциальным пользователям и данные из конфигурационного файла могут быть легко прочитаны.

Сделаем ещё небольшое изменение в файле.  
```xml
    <configSections>
        <section name="totalapi.log" type="TotalApi.Core.Api.FileLoggerConfiguration, TotalApi.Core" />
    </configSections>
    <totalapi.log>
        <files>
            <add file="con" level="0" />
        </files>
    </totalapi.log>
```
Этой настройкой мы, отключаем протоколирование в файл консоль, которое по умолчанию включено, чтобы сообщения протокола не мешали нашим рабочим сообщениям. При необходимости видеть сообщения протокола можно настроить протоколирование в файл. Подробнее о настройке протоколирования смотрите [здесь](https://github.com/TotalApi/SDK-dNet/blob/master/documentation/sdksettings.md#appllication-log-parameters).

Инициализация SDK TotalApi
--------------------------
Первой строкой нашего приложения должна быть:
```C#
TotalApiBootstrapper.Create();
```
При создании, бутстраппер загружает все необходимые сборки, выполняет настройку MEF-композиции, необходимой для работы приложения. Связывается с сервером TotalApi и получает необходимую информацию о подсистемах программного комплекса.   


Класс события
-------------
Алгоритм работы приложения будет очень простой. Все экземпляры приложения будут обмениваться определённым событием, специфичным именно для этой цели. 
Создадим класс `ChatEventObject` для этого события:
```C#
    public class ChatEventObject : TotalApiEventObject
    {
        public string Message { get; set; }

        public string UserName { get; set; }

        public ChatEventObject(string message)
        {
            Message = message;
            UserName = Thread.CurrentPrincipal.UserLogin();
        }
    }
``` 
В нашем событии мы будем передавать собственно текст сообщения и имя пользователя, передавшего сообщение (в нашем случае оно заполнятся значением логина пользователя). 
Все классы события **TotalApi** должны быть наследниками класса `TotalApiEventObject`. В этом случае они будут переданы между отдельными клиентами одного программного комплекса.


Отправка сообщения
------------------
После того как пользователь набрал текст сообщения необходимо создать класс события и отправить его всем подписчикам. Будем делать это в цикле пока не надоест:
```C#
    while (true)
    {
        var inputString = Console.ReadLine();
        CoreApi.EventManager.Publish(new ChatEventObject(inputString));
    }
``` 

Получение сообщения
-------------------
Клиент не получит сообщение если в его коде нет ни одного подписчика на него (в этом случае сообщение не будет даже послано этому клиенту). Обработчик событий **TotalApi** - это просто класс, реализующий интерфейс `IEvent<TEventObject>`. Такой класс должен быть описан обязательно либо в основной сборке приложения, либо в сборке, включённой в MEF-композицию приложения. Подробнее об этом [здесь](https://github.com/TotalApi/SDK-dNet/blob/master/documentation/sdkload.md#using-mef-platform):
```C#
    public class Subscriber : IEvent<ChatEventObject>
    {
        public static Subscriber Instance { get; } = new Subscriber();

        public void HandleEvent(ChatEventObject e)
        {
            if (e.UserName == Thread.CurrentPrincipal.UserLogin())
                ColorConsole.Do(ConsoleColor.Yellow, () =>
                {
                    Console.WriteLine($"\n    {e.EventTime} - Me > {e.Message}");
                });
            else
                ColorConsole.Do(ConsoleColor.Green, () =>
                {
                    Console.WriteLine($"\n{e.EventTime} - {e.UserName} > {e.Message}");
                });
            Console.Write("> ") ;
        }
    }
``` 
Код довольно простой. Подписчик оформлен в виде [синглтона](https://en.wikipedia.org/wiki/Singleton_pattern) (это очень упрощённая реализация [синглтона](https://en.wikipedia.org/wiki/Singleton_pattern), чтобы не перегружать пример кода - не используйте её в таком виде в реальном приложении).
Данный подписчик будет принимать как чужие, так и наши сообщения и для того, чтобы как-то выделить наши сообщения от чужих мы будем выводить их разными цветами. Для вывода цветного текста в консоль используется метод из SDK TotalApi `ColorConsole.Do()`. Проверку на то, наше это сообщение или нет будем осуществлять, проверяя значение `ChatEventObject.UserName` пришедшего сообщения с текущим логином.

Чтобы подписчик начал принимать сообщения достаточно вызвать метод:
```C#
CoreApi.EventManager.Subscribe(Subscriber.Instance);
```

Аутентификация пользователей
----------------------------
Всё готово, осталось только выполнить аутентификацию пользователей нашего чата. Для простоты будем сразу при старте запрашивать логин/пароль пользователя. Если такого пользователь ещё не зарегистрирован - сразу его и создадим. Если пароль введён неверно - повторим запрос.
```C#
    var isExists = false;
    while (!isExists)
    {
        Console.Write("Login: ");
        var userLogin = Console.ReadLine();
        Console.Write("Password: ");
        var userPassword = Console.ReadLine();
        try
        {
            Console.Write("Login: ");
            var userLogin = Console.ReadLine();
            Console.Write("Password: ");
            var userPassword = Console.ReadLine();
            try
            {
                // AppKey-authority is used in this call
                isExists = CoreApi.ApiUsers.IsExists(userLogin);
                if (!isExists)
                {
                    // User is not registered - auto register it and sign in
                    // AppKey-authority is used in this call
                    CoreApi.ApiUsers.Save(new ApiUser { Login = userLogin, Password = userPassword }, true);
                }
                // Set AppUser auth information
                TotalApiAuth.SetUserPassword(userLogin, userPassword);
                
                // Check whether auth information is valid
                // AppUser-authority is used in this call
                // If auth is invalid exception will be thrown
                isExists = CoreApi.ApiUsers.IsExists(userLogin);
                // If auth is valid - initialize subscriber, otherwise exception will be thrown
                CoreApi.EventManager.Subscribe(Subscriber.Instance);
            }
            catch (Exception e)
            {
                // Clear AppUser auth information
                TotalApiAuth.SetUserPassword(null, null);
                isExists = false;
                // Display error text
                ColorConsole.Do(ConsoleColor.Red, () => Console.WriteLine(e.FullMessage()));
            }

    }

```
Небольшое пояснение к данному коду. В самом начале после запроса пароля клиент использует [AppKey-авторизацию](https://github.com/TotalApi/SDK-dNet/blob/master/documentation/auth.md#authentication-types-of-totalapi). Она позволяет вызывать всего два метода TotalApi: `CoreApi.ApiUsers.IsExists(userLogin)` для проверки, есть ли такой пользователь или нет и `CoreApi.ApiUsers.Save(user)` для создания/регистрации нового пользователя.
Если пользователь существует - настраиваем [AppUser-авторизацию](https://github.com/TotalApi/SDK-dNet/blob/master/documentation/auth.md#authentication-types-of-totalapi)
```C#
    TotalApiAuth.SetUserPassword(userLogin, userPassword);
```
после чего последующий код клиента будет использовать [AppUser-авторизацию](https://github.com/TotalApi/SDK-dNet/blob/master/documentation/auth.md#authentication-types-of-totalapi) если переданный пароль верный. В противном случае будет выброшено исключение `Not authorized`.
В случае исключения необходимо переключиться обратно на [AppKey-авторизацию](https://github.com/TotalApi/SDK-dNet/blob/master/documentation/auth.md#authentication-types-of-totalapi).
```C#
    TotalApiAuth.SetUserPassword(null, null);
```

Заключение
----------
Это всё. Написав менее сотни строк кода, мы получили полноценный чат. 