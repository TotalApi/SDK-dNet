using System.ComponentModel.Composition;
using TotalApi.Core.ServiceContracts;
using TotalApi.SampleModule.Api;
using TotalApi.SampleModule.Api.ServiceContracts;
using TotalApi.Utils;

namespace TotalApi.SampleModule.Client
{
    [Export(typeof(ISampleModuleApi))]
    [Export(typeof(IFactory<ISampleService>))]
    internal class SampleModuleClient: ApiServiceProxy<ISampleService>, ISampleModuleApi
    {
        public string DoSomeStuff(string message)
        {
            return Create().DoSomeStuff(message);
        }
    }
}
