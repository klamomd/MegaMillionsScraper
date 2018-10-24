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
            // NOTE: 10/31/2017 is the start of the current rule set.
            //DateTime startDate = new DateTime(2010, 2, 2);
            DateTime startDate = new DateTime(2017, 10, 31);
            DateTime endDate = new DateTime(2018, 10, 23);
            List<DateTime> lottoDates = DateFetcher.FetchDateTimesInRange(startDate, endDate, true);
            List<MegaMillionsNumbers> megaNumbers = new List<MegaMillionsNumbers>();

            foreach (var dt in lottoDates)
            {
                Console.WriteLine(dt.ToString("M/d/yyyy"));
                megaNumbers.Add(WebpageScraper.GetNumbersForDate(dt));
            }

            Dictionary<int, int> whiteBallOccurrences = NumberCalculator.GetOccurrencesOfWhiteBalls(megaNumbers);
            Dictionary<int, int> megaBallOccurrences = NumberCalculator.GetOccurrencesOfMegaBalls(megaNumbers);

            var orderedWhiteBallOccurrences = whiteBallOccurrences.OrderByDescending(w => w.Value);
            var orderedMegaBallOccurrences = megaBallOccurrences.OrderByDescending(w => w.Value);

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

            Console.WriteLine("Done");
            Console.Read();
        }
    }
}
