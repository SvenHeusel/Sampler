using System;
using Sampler.Utilities;

namespace Sampler.Processing
{
    public class GridCalculator
    {
        private readonly int _sampleIntervalMinutes;

        public GridCalculator(int sampleIntervalMinutes)
        {
            _sampleIntervalMinutes = sampleIntervalMinutes;
        }

        public DateTime GetNextGridPoint(DateTime startOfSampling)
        {
            if (SamplingPointMatchesGridPoint(startOfSampling))
                return startOfSampling;

            var minuteOfSamplingStart = startOfSampling.Minute;
            var hourSectionOfSamplingStart = minuteOfSamplingStart / _sampleIntervalMinutes;
            var outputMinute = (hourSectionOfSamplingStart + 1) * _sampleIntervalMinutes;

            var adjustedGridPoint = startOfSampling.SetMinute(outputMinute).SetSecond(0);
            return adjustedGridPoint;
        }

        private bool SamplingPointMatchesGridPoint(DateTime startOfSampling)
        {
            var minuteIsGridMatch = startOfSampling.Minute % _sampleIntervalMinutes == 0;
            var secondIsGridMatch = startOfSampling.Second == 0;

            return minuteIsGridMatch && secondIsGridMatch;
        }
    }
}