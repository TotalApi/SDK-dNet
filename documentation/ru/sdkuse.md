Примеры использования TotalAPI SDK
===============================================
Базовые API
-----------------------------------
Для доступа к набору базовых API программного комплекса TotalApi, используется статический класс *CoreApi*. Ниже приведено их краткое описание с примерами использования:
### Репозитарий
Доступ к [распределённому репозиторию](http://) программного комплекса TotalApi, осуществляется посредством интерфейса *CoreApi.Repository:*
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
    
      // Modifying the entity
      device.Imei = "12345678901234";
      CoreApi.Repository.Save(device, false);
      
      // Searching an entity by Id - it should be found
      device = CoreApi.Repository.Find<Device>(deviceId);
      Debug.Assert(device.Id == deviceId, "Should be equal");
      
      // Searching an entity with a query (scalar result)
      device = CoreApi.Repository.ExecuteScalar<Device>(q => q.FirstOrDefault(d => d.Imei == "12345678901234"));
      Debug.Assert(device.Id == deviceId, "Should be equal");
     
      // Searching an entity with a query (multi result)
      device = CoreApi.Repository.ExecuteQuery<Device>(q => q.Where(d => d.Imei == "12345678901234")).First();
      Debug.Assert(device.Id == deviceId, "Should be equal");
      
      // Getting entities count (without any condition)
      var currentCount = CoreApi.Repository.Count<Device>();
      Debug.Assert(currentCount == startCount + 1, "Should be equal");
      
      // Deleting an entity
      CoreApi.Repository.Delete<Device>(deviceId);
      
      // Searching an entity by Id - it should not be found
      device = CoreApi.Repository.Find<Device>(deviceId);
      Debug.Assert(device == null, "Should be null");
      
      // Getting entities count (without any condition)
      currentCount = CoreApi.Repository.Count<Device>();
      Debug.Assert(currentCount == startCount, "Should be equal");
```
> Обратите внимание, что при вызове методов репозитария необходимо указывать тип бизнес-объекта, с которым мы работаем, за исключением тех случаев, когда этот тип заранее известен компилятору. Но не нужно указывать адрес (URL) подсистемы, которая обслуживает бизнес-объекты заданного типа (в нашем случае это телематическая подсистема metrixApi). 
> Также обратите внимание на то, каким образом передаются условия отбора при вызове методов *ExecuteScalar()*,* ExecuteQuery()* и *Count()* .  При передаче условий отбора таким образом фильтрация данных будет происходить на серверной стороне, что может существенно ускорить работу запроса. Однако в таком случае можно использовать только стандартные функции и операторы, а также реальные поля бизнес-объектов.  Другими словами, не любое условие можно указать в качестве параметра. Если же всё-таки это необходимо – воспользуйтесь отбором результата на клиентской стороне после выполнения запроса без этого условия.
```C#
    device = CoreApi.Repository.ExecuteQuery<Device>(). First(d => this.IsMatched(d));
```

### События
Работа с[ распределёнными событиями](http://) программного комплекса TotalApi, осуществляется посредством интерфейса *CoreApi.EventManager*: 

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
**Важное замечание:** 
> Подписчик события может поймать это событие даже если он было послано из другой подсистемы программного комплекса. В данном примере подписчик принимает события об изменении статуса устройств слежения которое генерируется в другом приложении и даже другом компьютере.
> Также и отправка события посредством *CoreApi.EventManager.Publish()* может отправить события ко всем подписчикам программного комплекса. Даже другому экземпляру этого приложения.
> Помимо стандартных событий, можно использовать свои собственные. Для этого достаточно создать собственный класс события, пронаследовав его от *TotalApiEventObject*. Такие события будут передаваться между приложениями одного программного комплекса TotalApi имеющих одинаковую аутентификацию. (Например, оба приложения используют ApiKey авторизацию с одинаковым ApiKey или AppUser-авторизации с одинаковым набором данных авторизации.

### Протоколирование
Для протоколирования программного комплекса TotalApi, используйте интерфейс *CoreApi.Logger*:

```C#
      CoreApi.Logger.Log("Found config file '{0}'. Loading...", fileName);
      CoreApi.Logger.Error(e);
      CoreApi.Logger.Warning("GC has disposed orphan subscriber.");
```
По умолчанию протоколирование ведётся в консоль и файл, согласно [параметрам протоколирования](sdksettings.md), указанным в конфигурационном файле. Однако можно легко добавить свои обработчики, путем экспорта одного или нескольких классов, реализующих интерфейс *ILogger*.


### Телеметрический API
Для доступа к [набору телеметрического API](http://) модуля metrixApi программного комплекса TotalApi, используется статический класс *TelematicsApi*. Ниже приведено их краткое описание с примерами использования:


    //TBD

