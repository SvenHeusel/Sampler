using System.Collections.Generic;
using System.Linq;
using Sampler.Container;
using Sampler.Enums;

namespace Sampler.Processing
{
    public class MeasurementSorter
    {
        public IOrderedEnumerable<Measurement> SortByTimeAscending(IEnumerable<Measurement> unsortedMeasurements)
        {
            return unsortedMeasurements.OrderBy(measurement => measurement.MeasurementTime);
        }

        public IEnumerable<Measurement> SortByType(IEnumerable<Measurement> unsortedMeasurements, MeasurementType type)
        {
            return unsortedMeasurements.Where(measurement => measurement.Type == type);
        }
    }
}