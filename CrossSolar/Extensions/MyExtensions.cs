using System;

namespace CrossSolar.Extensions
{
    public static class MyExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static void LogException(this Exception ec)
        {
            // log exception to database or file or elmah
        }
    }
}
