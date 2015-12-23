using System.ServiceModel;
using System.ServiceModel.Web;

namespace TotalApi.SampleModule.Api.ServiceContracts
{
    [ServiceContract(Name = "SampleService")]
    public interface ISampleService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/{message}")]
        string DoSomeStuff(string message);
    }
}
