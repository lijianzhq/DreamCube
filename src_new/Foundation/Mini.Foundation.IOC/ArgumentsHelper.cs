using System;

namespace Mini.Foundation.IOC
{
    public static class ArgumentsHelper
    {
        public static void ArgumentNotNull(Object value, String parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }

        public static void ArgumentNotNullOrEmpty(String value, String parameterName)
        {
            if (String.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(parameterName);
        }
    }
}
