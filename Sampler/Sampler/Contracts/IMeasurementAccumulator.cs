using System.Collections.Generic;
using Sampler.Container;

namespace Sampler.Contracts
{
    public interface IMeasurementAccumulator
    {
        IEnumerable<Measurement> CreateSampledMeasurements(IEnumerable<Measurement> inputMeasurements);
    }
}