using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillionsScraper
{
    public static class NumberCalculator
    {
        public static Dictionary<int, int> GetOccurrencesOfWhiteBalls(List<MegaMillionsNumbers> megaMillionsNumbers)
        {
            Dictionary<int, int> occurrences = new Dictionary<int, int>();
            for (int i = 1; i <= 70; i++)       // Values based on October 2017 rules change.
            {
                int totalOccurrencesOfNumber = megaMillionsNumbers.Count(m => m.Numbers.Contains(i));
                occurrences.Add(i, totalOccurrencesOfNumber);
            }

            return occurrences;
        }

        public static Dictionary<int, int> GetOccurrencesOfMegaBalls(List<MegaMillionsNumbers> megaMillionsNumbers)
        {
            Dictionary<int, int> occurrences = new Dictionary<int, int>();
            for (int i = 1; i <= 25; i++)       // Values based on October 2017 rules change.
            {
                int totalOccurrencesOfNumber = megaMillionsNumbers.Count(m => m.Megaplier == i);
                occurrences.Add(i, totalOccurrencesOfNumber);
            }

            return occurrences;
        }
    }
}
