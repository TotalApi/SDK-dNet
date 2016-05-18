using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using TotalApi.Billing;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Core.ServiceContracts.RepositoryApi;
using TotalApi.Utils.Extensions;
using WebApp.Api;

namespace WebApp.TotalApi
{
    [System.Web.Http.RoutePrefix(Consts.RoutePrefix + "RepositoryApi/{typeId}")]
    public class RepositoryApiController : AppApiController, IRepositoryApiService
    {
        private static Type GetEntityType(string typeId)
        {
            var res = typeId.GetRepositoryType();
            if (res == null)
                throw new ApplicationException("{0}: The entity class is not registered.".Fmt(typeId));
            return res;
        }
        private static IEntityObject GetEntityObject(string typeId, object src)
        {
            var resType = typeId.GetRepositoryType();
            if (resType == null)
                throw new ApplicationException("{0}: The entity class is not registered.".Fmt(typeId));
            var res = ((JObject) src).ToObject(resType);
            return (IEntityObject)res;
        }

        public static T Sanity<T>(T entity)
        {
            if (entity is ITotalApiEntityObject)
            {
                (entity as ITotalApiEntityObject).ApiKey = null;
            }
            else if (entity is Application)
            {
                (entity as Application).ApiKey = null;
                (entity as Application).SubSystems.ForEach(ss => Sanity(ss));
            }
            else if (entity is ApiUser)
            {
                (entity as ApiUser).Application = Sanity((entity as ApiUser).Application);
                (entity as ApiUser).Password = null;
            }
            else if (entity is SubSystem)
            {
                (entity as SubSystem).SSID = null;
                (entity as SubSystem).ApiKeys.Clear();
            }
            return entity;
        }

        public int Count(string typeId)
        {
            throw new NotImplementedException();
        }

        [System.Web.Http.HttpGet, System.Web.Http.Route("")]
        public object[] All(string typeId)
        {
            // The code below is equal to the next call 
            //    return CoreApi.Repository.ExecuteQuery<{Type}>();
            // OData parameters in the query will be passed futher to the service and will be applied on the server side
            var entityType = GetEntityType(typeId);
            var mi = CoreApi.Repository.GetType().GetMethod("ExecuteQuery");
            var typedMi = mi.MakeGenericMethod(entityType);
            return ((IEnumerable<object>)typedMi.Invoke(CoreApi.Repository, new object[] { null })).Select(Sanity).ToArray();
        }

        [System.Web.Http.HttpGet, System.Web.Http.Route("{id}")]
        public object Find(string typeId, string id)
        {
            // The code below is equal to the next call 
            //    return CoreApi.Repository.Find<{Type}>(id);
            var entityType = GetEntityType(typeId);
            var mi = CoreApi.Repository.GetType().GetMethod("Find");
            var typedMi = mi.MakeGenericMethod(entityType);
            return Sanity(typedMi.Invoke(CoreApi.Repository, new object[] { id }));
        }

        [System.Web.Http.HttpPost, System.Web.Http.Route("")]
        public object Create(string typeId, object entity)
        {
            return Sanity(CoreApi.Repository.Save(GetEntityObject(typeId, entity), true));
        }

        [System.Web.Http.HttpPut, System.Web.Http.Route("")]
        public object Update(string typeId, object entity)
        {
            return Sanity(CoreApi.Repository.Save(GetEntityObject(typeId, entity), false));
        }

        [System.Web.Http.HttpDelete, System.Web.Http.Route("{id}")]
        public bool Delete(string typeId, string id)
        {
            // The code below is equal to the next call 
            //    return CoreApi.Repository.Delete<{Type}>(id);
            var entityType = GetEntityType(typeId);
            var mi = CoreApi.Repository.GetType().GetMethod("Delete");
            var typedMi = mi.MakeGenericMethod(entityType);
            return (bool)typedMi.Invoke(CoreApi.Repository, new object[] { id });
        }

    }
}