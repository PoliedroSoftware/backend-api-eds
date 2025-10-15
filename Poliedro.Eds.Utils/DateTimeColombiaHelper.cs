using System;
using System.Runtime.InteropServices;

namespace Poliedro.Eds.Utils
{
    public static class DateTimeColombiaHelper
    {
        private static readonly TimeZoneInfo ColombiaTimeZone = GetColombiaTimeZone();

        private static TimeZoneInfo GetColombiaTimeZone()
        {
            // Windows uses "SA Pacific Standard Time", Linux/macOS use "America/Bogota"
            var windowsId = "SA Pacific Standard Time";
            var ianaId = "America/Bogota";

            try
            {
                var id = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsId : ianaId;
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
                // Fallbacks: try the other id
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ianaId : windowsId);
                }
                catch
                {
                    // As a last resort, return UTC to avoid crashes
                    return TimeZoneInfo.Utc;
                }
            }
            catch (InvalidTimeZoneException)
            {
                return TimeZoneInfo.Utc;
            }
        }

        public static DateTime ToColombiaTime(DateTime dateTime)
        {
            DateTime utc;
            if (dateTime.Kind == DateTimeKind.Unspecified)
                utc = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            else if (dateTime.Kind == DateTimeKind.Local)
                utc = dateTime.ToUniversalTime();
            else
                utc = dateTime; // already UTC

            var colombia = TimeZoneInfo.ConvertTimeFromUtc(utc, ColombiaTimeZone);
            return DateTime.SpecifyKind(colombia, DateTimeKind.Unspecified);
        }

        public static DateTime FromColombiaTime(DateTime colombiaDateTime)
        {
            var specified = DateTime.SpecifyKind(colombiaDateTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(specified, ColombiaTimeZone);
        }

        public static DateTime NowColombia()
        {
            return ToColombiaTime(DateTime.UtcNow);
        }
    }
}
