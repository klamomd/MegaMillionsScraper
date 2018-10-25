using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillionsScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            // NOTE: 10/31/2017 is the start of the current rule set. Any dates before that will have different number ranges for white balls and mega balls, which complicates things.
            // For proper functionality, do not use with a startDate prior to 10/31/2017, or you will need to change some of the for loops used (particularly in NumberCalculator).
            DateTime startDate = new DateTime(2017, 10, 31);
            DateTime endDate = new DateTime(2018, 10, 23);

            // The drawing date immediately after 10/31/17 was 3 days after, so we use true for incrementByThreeFirst.
            List<DateTime> lottoDates = DateFetcher.FetchDateTimesInRange(startDate, endDate, true);
            List<MegaMillionsNumbers> megaNumbers = new List<MegaMillionsNumbers>();

            // Loop through each DateTime, print it to the console to show we are working on it, and then scrape the webpage for the numbers and add it to our megaNumbers list.
            foreach (var dt in lottoDates)
            {
                Console.WriteLine(dt.ToString("M/d/yyyy"));
                megaNumbers.Add(WebpageScraper.GetNumbersForDate(dt));
            }

            // Get the number of occurrences for both types of numbers.
            Dictionary<int, int> whiteBallOccurrences = NumberCalculator.GetOccurrencesOfWhiteBalls(megaNumbers);
            Dictionary<int, int> megaBallOccurrences = NumberCalculator.GetOccurrencesOfMegaBalls(megaNumbers);

            // Order the number of occurrences for both types of numbers (for easier viewing later).
            var orderedWhiteBallOccurrences = whiteBallOccurrences.OrderByDescending(w => w.Value);
            var orderedMegaBallOccurrences = megaBallOccurrences.OrderByDescending(w => w.Value);

            // Write the data into a file called Results.txt.
            using (StreamWriter file = new StreamWriter(@"./Results.txt"))
            {
                file.WriteLine("White ball occurrences:");
                for (int i = 1; i <= whiteBallOccurrences.Keys.Max(); i++)
                {
                    file.WriteLine(string.Format("{0}: {1}", i, whiteBallOccurrences[i]));
                }

                file.WriteLine();

                file.WriteLine("Mega ball occurrences:");
                for (int i = 1; i <= megaBallOccurrences.Keys.Max(); i++)
                {
                    file.WriteLine(string.Format("{0}: {1}", i, megaBallOccurrences[i]));
                }
                file.WriteLine();

                file.WriteLine("Ordered white ball occurrences:");
                foreach (var kv in orderedWhiteBallOccurrences)
                {
                    file.WriteLine(string.Format("{0}: {1}", kv.Key, kv.Value));
                }
                file.WriteLine();

                file.WriteLine("Ordered mega ball occurrences:");
                foreach (var kv in orderedMegaBallOccurrences)
                {
                    file.WriteLine(string.Format("{0}: {1}", kv.Key, kv.Value));
                }
                file.WriteLine();
            }

            // Let the user know we're done and wait for them to close out.
            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
