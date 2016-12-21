using System;
using System.Collections.Generic;

namespace mvHub
{
    public static class MvConversion
    {
        private static readonly Dictionary<string, MvCustomConversion> ConvTable =
             new Dictionary<string, MvCustomConversion>();

        private static readonly object SyncRoot = new object();
        private static DateTime _baseDate1 = new DateTime(1968, 1, 1);
        private static DateTime _baseDate2 = new DateTime(1968, 1, 2);

        public static DateTime ToDate(string mvString)
        {

            var basedate = _baseDate1.Ticks;
            var date2 = _baseDate2.Ticks;
            var factor = date2 - basedate;

            basedate -= factor;

            int idate;
            int.TryParse(mvString, out idate);
            var chdate = (idate * factor) + basedate;

            return new DateTime(chdate);
        }
        public static DateTime ToTime(string mvString)
        {

            double ttime;
            double.TryParse(mvString, out ttime);
            var itime = (int)(ttime * 1000);
            return new DateTime().AddMilliseconds(itime);
        }
        public static int ToInt(string mvString)
        {
            try
            {
                double rtn;
                double.TryParse(mvString, out rtn);
                return Convert.ToInt32(rtn + .5);
            }
            catch
            {
                return 0;
            }
        }
        public static long ToLong(string mvString)
        {
            try
            {
                double rtn;
                double.TryParse(mvString, out rtn);
                return Convert.ToInt64(rtn + .5);
            }
            catch
            {
                return 0;
            }
        }
        public static double ToDouble(string mvString)
        {
            try
            {
                double rtn;
                double.TryParse(mvString, out rtn);
                return rtn;
            }
            catch
            {
                return 0;
            }
        }
        public static string Oconv(string value, string conversion)
        {
            var conv = conversion.ToUpper();
            lock (SyncRoot)
            {
                return ConvTable.ContainsKey(conv) ? ConvTable[conv].OConv(value) : "";
            }
        }
        public static string Iconv(string value, string conversion)
        {

            var conv = conversion.ToUpper();
            lock (SyncRoot)
            {
                return ConvTable.ContainsKey(conv) ? ConvTable[conv].Conv(value) : "";
            }
        }
        /// <summary>
        /// Returns True if add new to table. 
        /// </summary>
        public static bool RegisterCustomConversion(MvCustomConversion cConv)
        {
            var conv = cConv.SafeName.ToUpper();
            lock (SyncRoot)
            {
                if (ConvTable.ContainsKey(conv))
                {
                    ConvTable[conv] = cConv;
                    return false;
                }
                ConvTable.Add(conv, cConv);
                return true;
            }
        }

    }
}
