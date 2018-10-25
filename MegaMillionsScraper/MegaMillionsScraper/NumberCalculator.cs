using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillionsScraper
{
    public static class NumberCalculator
    {
        // Given a List of MegaMillionsNumbers, this parses through it and records the number of occurrences of all white ball numbers (1-70 (which is the white ball range as of 10/31/17)).
        // Returns a Dictionary where the key is the number and the value is the number of occurrences found.
        public static Dictionary<int, int> GetOccurrencesOfWhiteBalls(List<MegaMillionsNumbers> megaMillionsNumbers)
        {
            Dictionary<int, int> occurrences = new Dictionary<int, int>();
            for (int i = 1; i <= 70; i++)       // Values based on October 2017 rules change.
            {
                // Since each number can only occur once in each drawing, we simply have to count the number of drawings that contain that number in their white ball list.
                int totalOccurrencesOfNumber = megaMillionsNumbers.Count(m => m.Numbers.Contains(i));
                occurrences.Add(i, totalOccurrencesOfNumber);
            }

            return occurrences;
        }

        // Given a List of MegaMillionsNumbers, this parses through it and records the number of occurrences of all mega ball numbers (1-25 (which is the mega ball range as of 10/31/17)).
        // Returns a Dictionary where the key is the number and the value is the number of occurrences found.
        public static Dictionary<int, int> GetOccurrencesOfMegaBalls(List<MegaMillionsNumbers> megaMillionsNumbers)
        {
            Dictionary<int, int> occurrences = new Dictionary<int, int>();
            for (int i = 1; i <= 25; i++)       // Values based on October 2017 rules change.
            {
                // Simply count the number of drawings where the Megaplier is the specified number.
                int totalOccurrencesOfNumber = megaMillionsNumbers.Count(m => m.Megaplier == i);
                occurrences.Add(i, totalOccurrencesOfNumber);
            }

            return occurrences;
        }
    }
}
