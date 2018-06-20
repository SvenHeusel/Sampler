using System.ComponentModel;

namespace Sampler.Enums
{
    public enum MeasurementType
    {
        [Description("HR")]
        HeartRate,

        [Description("TEMP")]
        Temperature,

        [Description("SPO2")]
        SpO2,
    }
}