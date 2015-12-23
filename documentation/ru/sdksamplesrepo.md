Репозиторий
===================
Доступ к распределённому репозиторию программного комплекса TotalApi, осуществляется посредством интерфейса *CoreApi.Repository*:
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
  device = CoreApi.Repository
      .ExecuteScalar<Device>(q => q.FirstOrDefault(d => d.Imei == "12345678901234"));
  Debug.Assert(device.Id == deviceId, "Should be equal");
 
  // Searching an entity with a query (multi result)
  device = CoreApi.Repository
      .ExecuteQuery<Device>(q => q.Where(d => d.Imei == "12345678901234"))
      .First();
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
Обратите внимание, что при вызове методов репозитория необходимо указывать тип бизнес-объекта, с которым мы работаем, за исключением тех случаев, когда этот тип заранее известен компилятору. Но не нужно указывать адрес (URL) подсистемы, которая обслуживает бизнес-объекты заданного типа (в нашем случае это телематическая подсистема metrixApi). 
Также обратите внимание на то, каким образом передаются условия отбора при вызове методов `ExecuteScalar()`, `ExecuteQuery()` и `Count()` .  При передаче условий отбора таким образом фильтрация данных будет происходить на серверной стороне, что может существенно ускорить работу запроса. Однако в таком случае можно использовать только стандартные функции и операторы, а также реальные поля бизнес-объектов.  Другими словами, не любое условие можно указать в качестве параметра. Если же всё-таки это необходимо – воспользуйтесь отбором результата на клиентской стороне после выполнения запроса без этого условия.

```C#
device = CoreApi.Repository.ExecuteQuery<Device>(). First(d => this.IsMatched(d));
```