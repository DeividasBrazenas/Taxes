using System;

namespace Taxes.Service.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsBewteenTwoDates(this DateTime dt, DateTime start, DateTime end)
        {
            return dt >= start || dt <= end;
        }
    }
}
