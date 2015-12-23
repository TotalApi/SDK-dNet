using System.ComponentModel.Composition;
using TotalApi.Core.Api.ControlApi;
using TotalApi.Core.ServiceContracts;
using TotalApi.SampleModule.Api.ServiceContracts;

namespace TotalApi.SampleModule
{
    [Export(typeof(IModuleInitializer))]
    class Startup : IModuleInitializer
    {
        public void Init()
        {
            TotalApiServiceHost<SampleModuleService>.Open<ISampleService>();
        }
    }
}