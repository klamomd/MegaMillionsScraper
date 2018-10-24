using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillionsScraper
{
    static class DateFetcher
    {
        public static List<DateTime> FetchDateTimesInRange(DateTime startDate, DateTime endDate, bool incrementByThreeFirst)
        {
            if (startDate == null) throw new ArgumentNullException("startDate");
            if (endDate == null) throw new ArgumentNullException("endDate");
            if (endDate.CompareTo(startDate) <= 0) throw new ArgumentException("startDate must be before endDate.");

            List<DateTime> dates = new List<DateTime>();
            bool incrementByThree = incrementByThreeFirst;

            DateTime currentDate = startDate;
            do
            {
                dates.Add(currentDate);

                // Increment by 3 days.
                if (incrementByThree)
                {
                    currentDate = currentDate.AddDays(3);
                }
                // Increment by 4 days.
                else
                {
                    currentDate = currentDate.AddDays(4);
                }

                incrementByThree = !incrementByThree;
            }
            while (currentDate.CompareTo(endDate) <= 0);

            return dates;
        }
    }
}
