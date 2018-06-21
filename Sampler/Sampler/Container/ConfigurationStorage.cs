using Sampler.Contracts;
using Sampler.Utilities;

namespace Sampler.Container
{
    public class ConfigurationStorage : IConfigurationStorage
    {
        public int SamplingGridMinutes => Globals.SamplingGridMinutes;
    }
}