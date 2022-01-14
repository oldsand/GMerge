using System;
using ArchestrA.GRAccess;

namespace GServer.Archestra.Extensions
{
    public static class TimeExtensions
    {
        /// <summary>
        /// Converts the VBFileTime type to a standard .NET DateTime type.
        /// </summary>
        /// <param name="time">The current VBFileTime</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime(this VBFILETIME time)
        {
            var high = (ulong) time.HighDateTime;
            var low = (uint) time.LowDateTime;
            var fileTime = (long) ((high << 32) + low);
            return DateTime.FromFileTime(fileTime);
        }

        /// <summary>
        /// Converts the DateTime type to a VBFileTime type.
        /// </summary>
        /// <param name="dateTime">The current DateTime</param>
        /// <returns>VBFILETIME</returns>
        public static VBFILETIME ToVbFileTime(this DateTime dateTime)
        {
            var fileTime = dateTime.ToFileTime();

            return new VBFILETIME
            {
                HighDateTime = (int) (fileTime >> 32),
                LowDateTime = (int) (fileTime & uint.MaxValue)
            };
        }

        /// <summary>
        /// Converts the VbLargeInteger type to a TimeSpan type.
        /// </summary>
        /// <param name="value">The current VbLargeInteger</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan ToTimeSpan(this VB_LARGE_INTEGER value)
        {
            var high = (ulong) value.HighPart;
            var low = (uint) value.LowPart;
            var time = (long) ((high << 32) + low);
            return TimeSpan.FromTicks(time);
        }
        
        /// <summary>
        /// Converts the TimeSpan Type to VBLargeInteger Type
        /// </summary>
        /// <param name="timeSpan">The current TimeSpan</param>
        /// <returns>VB_LARGE_INTEGER</returns>
        public static VB_LARGE_INTEGER ToVbLargeInteger(this TimeSpan timeSpan)
        {
            var ticks = timeSpan.Ticks;
            
            return new VB_LARGE_INTEGER
            {
                HighPart = (int) (ticks >> 32),
                LowPart = (int) (ticks & uint.MaxValue)
            };
        }
    }
}