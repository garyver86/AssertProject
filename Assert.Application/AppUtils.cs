using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application
{
    public class AppUtils
    {
        public static int[] GetTimeElapsed(DateTime startDate)
        {
            DateTime endDate = DateTime.UtcNow;

            if (startDate > endDate)
            {
                return new int[] { 0, 0, 0 };
            }

            int years = 0;
            int months = 0;
            int days = 0;

            years = endDate.Year - startDate.Year;
            if (endDate.Month < startDate.Month || (endDate.Month == startDate.Month && endDate.Day < startDate.Day))
            {
                years--;
            }

            DateTime tempDateForMonths = startDate.AddYears(years);
            months = 0;
            while (tempDateForMonths.AddMonths(1) <= endDate)
            {
                tempDateForMonths = tempDateForMonths.AddMonths(1);
                months++;
            }

            days = (endDate - tempDateForMonths).Days;

            int[] result = new int[3] { days, months, years };

            return result;
        }
    }
}
