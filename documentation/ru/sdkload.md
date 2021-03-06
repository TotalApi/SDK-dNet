[English](../sdkload.md)

Использование платформы MEF
--------------------------------------------------------
TotalAPI SDK для своей работы использует платформу [MEF](http://bit.do/bE73V).
Следующий вызов производит компоновку композиции:
```C#    
    TotalApiBootstrapper.AppModulesSearchPattern = "myApp.*.dll";
    TotalApiBootstrapper.Create();
```
** Хорошая новость: **  теперь вы можете использовать [MEF](http://bit.do/bE73V) в своём приложении для своих классов без дополнительного конфигурирования, если названия сборок, содержащих экспортируемые классы, соответствуют маске, установленной в свойстве `AppModulesSearchPattern`. Если данный параметр не установлен – никакие другие сборки не будут автоматически подключены. (Сборка из которой была произведена инициализация [MEF](http://bit.do/bE73V) будет подключена к композиции в любом случае). 
** Важно **
> - Не устанавливайте маску \*.dll, т.к. в этом случае будет сделана попытка добавить в композицию все сборки (в том числе и системные), что может привести к очень долгому старту программы.
> - Не меняйте названия файлов SDK, т.к. они включаются в композицию по жестко заданной маске  TotalApi.\*.dll
> - По умолчанию модули для загрузке будут искаться вкаталоге с исполнимым файлом и рекурсивно во всех подкаталогах.