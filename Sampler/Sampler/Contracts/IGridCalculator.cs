using System;

namespace Sampler.Contracts
{
    public interface IGridCalculator
    {
        DateTime GetNextGridPoint(DateTime startOfSampling);
    }
}