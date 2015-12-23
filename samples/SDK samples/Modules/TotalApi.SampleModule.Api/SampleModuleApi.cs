using TotalApi.Utils;

namespace TotalApi.SampleModule.Api
{
    public static class SampleModuleApi
    {
        public static ISampleModuleApi SampleModule { get { return LazyIoC<ISampleModuleApi>.Instance; } }
    }

    public interface ISampleModuleApi
    {
        string DoSomeStuff(string message);
    }
}
