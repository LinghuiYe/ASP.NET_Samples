using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDP
{
    public static class TimeConverter
    {
        /// <summary>
        /// Converts Uni's epoch time to C# DateTime value
        /// </summary>
        /// <param name="EpochValue"></param>
        /// <returns>DateTime</returns>
        public static DateTime EpochToDateTime(UInt32 EpochValue)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (EpochValue >= 0)
            {
                return dt.AddSeconds(EpochValue);
            }
            else
            {
                return dt;
            }
        }

        /// <summary>
        /// Converts DateTime to Unix's Epoch Time (Seconds Since 1,1,1970)
        /// </summary>
        /// <param name="DateTimeValue"></param>
        /// <returns>Time Since Epoch (Seconds)</returns>
        public static int DateTimeToEpoch(DateTime DateTimeValue)
        {
            try
            {
                return (int)DateTimeValue.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            }
            catch (OverflowException)
            {
                return -1;
            }
        }

        /// <summary>
        /// Returns seconds since epoch as bytes in a 6 byte array
        /// </summary>
        /// <returns>Padded Byte Array (6 Bytes) of Current UTC Date Time</returns>
        public static byte[] CurrentUTCDateTimeToByteArray()
        {
            int secondsSinceEpoch = DateTimeToEpoch(DateTime.UtcNow);
            if (secondsSinceEpoch != -1)
            {
                byte[] timepacket = new byte[6];
                byte[] timeinbytes = BitConverter.GetBytes(secondsSinceEpoch);
                timepacket[2] = timeinbytes[0];
                timepacket[3] = timeinbytes[1];
                timepacket[4] = timeinbytes[2];
                timepacket[5] = timeinbytes[3];
                return timepacket;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns seconds since epoch as bytes in a 6 byte array
        /// </summary>
        /// <param name="date"></param>
        /// <returns>Padded Byte Array (6 Bytes) of Date Time Passed In</returns>
        public static Byte[] DateTimeToByteArray(DateTime date)
        {
            int seconds = DateTimeToEpoch(date);
            if (seconds != -1)
            {
                byte[] timepacket = new byte[6];
                byte[] timeinbytes = BitConverter.GetBytes(seconds);
                timepacket[2] = timeinbytes[0];
                timepacket[3] = timeinbytes[1];
                timepacket[4] = timeinbytes[2];
                timepacket[5] = timeinbytes[3];
                return timepacket;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Truncates a DateTime Object to the nearest TimeSpan.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>DateTime truncated to the TimeSpan Specified</returns>
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) return dateTime; // Or could throw an ArgumentException
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }
}
