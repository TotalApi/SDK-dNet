using TotalApi.Core.Authentication;
using TotalApi.DataAccess;
using TotalApi.SampleModule.Api;
using TotalApi.SampleModule.Api.ServiceContracts;

namespace TotalApi.SampleModule
{
    [Authorized(Role = TotalApiAuthenticationType.ApiKeyUser)]
    class SampleModuleService: TotalApiServiceBase, ISampleService
    {
        public string DoSomeStuff(string message)
        {
            return SampleModuleApi.SampleModule.DoSomeStuff(message);
        }
    }
    
}
