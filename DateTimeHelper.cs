using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpUtils
{
    class DateTimeHelper
    {
        #region 时间戳与DateTime互转
        public static long DateTimeToTimeStamp(DateTime dt, bool bSecond)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            TimeSpan ts = dt - startTime;
            if (bSecond)
                return Convert.ToInt64(ts.TotalSeconds);
            else
                return Convert.ToInt64(ts.TotalMilliseconds);
        }
        public static DateTime TimeStampToDateTime(long timestamp, bool bSecond)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            if (bSecond)
                return startTime.AddSeconds(timestamp);
            else
                return startTime.AddMilliseconds(timestamp);
        }

        public static double DateTimeToTimeVal(DateTime dateTime)
        {
            DateTime timeD = DateTime.Parse("1970-01-01 08:00:00");
            var data = (dateTime - timeD).TotalMilliseconds;
            return data;
        }
        public static DateTime TimeValToDateTime(double timeVal)
        {
            DateTime timeD = DateTime.Parse("1970-01-01 08:00:00");
            var data = timeD.AddMilliseconds(timeVal);
            return data;
        }
        #endregion
    }
}
