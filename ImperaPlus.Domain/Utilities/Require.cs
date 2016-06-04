using System;
using System.Diagnostics;

namespace ImperaPlus.Domain.Utilities
{
    public static class Require
    {
        [DebuggerStepThrough]
        public static void NotNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void NotEmpty(Guid id, string parameterName)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(parameterName);
            }
        }
    }
}
