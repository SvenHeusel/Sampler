using System.Collections.Generic;
using System.Linq;
using Sampler.Container;
using Sampler.Enums;

namespace Sampler.Contracts
{
    public interface IMeasurementSorter
    {
        IOrderedEnumerable<Measurement> SortByTimeAscending(IEnumerable<Measurement> unsortedMeasurements);

        IEnumerable<Measurement> SortByType(IEnumerable<Measurement> unsortedMeasurements, MeasurementType type);
    }
}