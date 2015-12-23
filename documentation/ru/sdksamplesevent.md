События
================
Работа с распределёнными событиями программного комплекса TotalApi, осуществляется посредством интерфейса *CoreApi.EventManager*: 
```C#
class Subscriber : IEvent, IEvent<OnPing>, IEvent<OnDeviceStatusChanged>
{
    public Subscriber()
    {
        CoreApi.EventManager.Subscribe(this);
    }

    public void HandleEvent(object e)
    {
       Console.WriteLine("1. IEvent: " + e.GetType().FullName));
    }

    public void HandleEvent(OnPing e)
    {
        Console.WriteLine("2. OnPing: " + e.Content));
    }

    public void HandleEvent(OnDeviceStatusChanged e)
    {
        Console.WriteLine("3. OnDeviceStatusaChanged: {0}".Fmt(e.DeviceStatus.Id)));
    }
}  
    ...
    CoreApi.EventManager.Publish(new OnPing("Test"));
    ...
```
**Важное замечание:** Подписчик события может поймать это событие даже если он было послано из другой подсистемы программного комплекса. В данном примере подписчик принимает события об изменении статуса устройств слежения которое генерируется в другом приложении и даже другом компьютере.
Также и отправка события посредством `CoreApi.EventManager.Publish()` может отправить события ко всем подписчикам программного комплекса. Даже другому экземпляру этого приложения.
Помимо стандартных событий, можно использовать свои собственные. Для этого достаточно создать собственный класс события, пронаследовав его от `TotalApiEventObject`. Такие события будут передаваться между приложениями одного программного комплекса TotalApi имеющих одинаковую аутентификацию. (Например, оба приложения используют ApiKey авторизацию с одинаковым ApiKey или AppUser-авторизации с одинаковым набором аутентификационных данных.