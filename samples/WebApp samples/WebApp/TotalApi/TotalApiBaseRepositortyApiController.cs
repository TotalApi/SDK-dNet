using System.Linq;
using System.Web.Http;
using TotalApi.Core;
using TotalApi.Core.Api;
using TotalApi.Core.ServiceContracts.RepositoryApi;
using WebApp.Api;

namespace WebApp.TotalApi
{
    /// <summary>
    /// Базовый класс для WebApi контроллеров TotalApi-сервисов, реализующих RepositoryApi.
    /// </summary>
    public class TotalApiBaseRepositortyApiController<TEntityObjectId> : AppApiController, IBaseRepositoryService<TEntityObjectId> where TEntityObjectId : class, IEntityObjectId, new()
    {
        public int Count()
        {
            return CoreApi.Repository.Count<TEntityObjectId>();
        }

        [HttpGet, Route("")]
        public virtual TEntityObjectId[] All()
        {
            return CoreApi.Repository.ExecuteQuery<TEntityObjectId>().ToArray();
        }

        [HttpGet, Route("{id}")]
        public virtual TEntityObjectId Find(string id)
        {
            return CoreApi.Repository.Find<TEntityObjectId>(id);
        }

        [HttpPost, Route("")]
        public virtual TEntityObjectId Create([FromBody]TEntityObjectId entity)
        {
            return CoreApi.Repository.Save(entity, true);
        }

        [HttpPut, Route("")]
        public virtual TEntityObjectId Update([FromBody]TEntityObjectId entity)
        {
            return CoreApi.Repository.Save(entity, false);
        }

        [HttpDelete, Route("{id}")]
        public virtual bool Delete(string id)
        {
            return CoreApi.Repository.Delete<TEntityObjectId>(id);
        }
    }
}