using System;
using System.ComponentModel;

namespace Sampler.Utilities
{
    public static class Extensions
    {
        public static string GetDescription<T>(this T enumValue)
        {
            var inputType = typeof(T);
            if (!inputType.IsEnum)
                throw new InvalidOperationException($"Type '{inputType}' is not an Enum and thus invalid for this procedure.");

            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            const bool retrieveInheritedAttributes = true;
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), !retrieveInheritedAttributes);
            return attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();
        }

        public static DateTime SetMinute(this DateTime input, int newMinuteValue)
        {
            var inputWithResetMinuteValue = new DateTime(input.Year, input.Month, input.Day, input.Hour, 0, input.Second);
            return inputWithResetMinuteValue.AddMinutes(newMinuteValue);
        }

        public static DateTime SetSecond(this DateTime input, int newSecondValue)
        {
            var inputWithResetSecondValue = new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, 0);
            return inputWithResetSecondValue.AddSeconds(newSecondValue);
        }
    }
}