using System;
using System.Collections.Generic;
using System.Linq;

namespace Sampler.Utilities
{
    public static class EnumUtilities
    {
        public static IEnumerable<T> GetEnumValues<T>()
        {
            var inputType = typeof(T);
            if (!inputType.IsEnum)
                throw new InvalidOperationException($"Type '{inputType}' is not an Enum type and thus invalid for this operation.");

            return Enum.GetValues(inputType).Cast<T>();
        }
    }
}