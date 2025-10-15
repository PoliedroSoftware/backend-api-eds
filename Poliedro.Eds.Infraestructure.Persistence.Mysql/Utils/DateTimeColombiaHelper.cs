using System;
using System.Runtime.InteropServices;

namespace Poliedro.Eds.Infraestructure.Persistence.Mysql.Utils
{
    public static class DateTimeColombiaHelper
    {
        private static readonly TimeZoneInfo ColombiaTimeZone = GetColombiaTimeZone();

        private static TimeZoneInfo GetColombiaTimeZone()
        {
            var windowsId = "SA Pacific Standard Time";
            var ianaId = "America/Bogota";

            try
            {
                var id = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? windowsId : ianaId;
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ianaId : windowsId);
                }
                catch
                {
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
