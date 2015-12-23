using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.SampleModule.Api;
using TotalApi.SampleModule.Api.Models;

namespace TotalApi.SampleModule
{
    [Export(typeof(ISampleModuleApi))]
    class SampleModule: ISampleModuleApi
    {
        public string DoSomeStuff(string message)
        {
            return string.Format("Done doing {0} stuff", message);
        }
    }

    //[Export(typeof(ISampleObjectModel))]
    class SampleDataObjects : RepositoryId<SampleObjectModel>, ISampleObjectModel
    {
        public int SomeAdditionalProperty { get; set; }
    }

    [Export(typeof (ISampleDbContext)), PartCreationPolicy(CreationPolicy.NonShared)]
    internal class SampleDbContext : ISampleDbContext
    {
        public void Dispose()
        {
            //do nothing
        }

        public object Self
        {
            get
            {
                return this;
            }
        }

        public IQueryable<TEntityObject> DbSet<TEntityObject>() where TEntityObject : class, IEntityObject, new()
        {
            return new List<TEntityObject>()
            {
                new SampleObjectModel()
                {
                    SampleObjectId = "1",
                    TestProp = 111
                } as TEntityObject
            }.AsQueryable();
        }

        public IEntityObject Attach(IEntityObject entity, bool isNew)
        {
            throw new NotImplementedException();
        }

        public IEntityObject Save(IEntityObject entity, bool isNew)
        {
            throw new NotImplementedException();
        }

        public bool Delete(IEntityObject entity)
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public TEntityObjectId Find<TEntityObjectId>(string id) where TEntityObjectId : class, IEntityObjectId, new()
        {
            throw new NotImplementedException();
        }

        public bool Delete<TEntityObjectId>(string id) where TEntityObjectId : class, IEntityObjectId, new()
        {
            throw new NotImplementedException();
        }

        public bool LazyLoadingEnabled { get; set; }
    }
}
