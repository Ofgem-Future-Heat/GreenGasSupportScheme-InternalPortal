using System;
using System.Globalization;
using Microsoft.VisualBasic;

namespace InternalPortal.Extensions
{
    public static partial class DateFormatExtensions
    {
        public static string ToOfgemShortDate(this string date)
        {
            const string dateFormat = "dd MMM yyyy HH:mm tt";

            string[] formats = { "yyyy-MM-ddTHH:mm:ss", "dd MMM yyyy", "dd MMM yyyy HH:mm tt" };

            if (DateTime.TryParseExact(date, formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out DateTime result))
            {
                return result.ToString(dateFormat);
            }

            return DateTime.Now.ToString(dateFormat);
        }
    }
}
