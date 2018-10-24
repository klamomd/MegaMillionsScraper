using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillionsScraper
{
    public class MegaMillionsNumbers
    {
        public MegaMillionsNumbers(List<int> numbers, int megaplier)
        {
            if (numbers == null) throw new ArgumentNullException("numbers", "numbers cannot be null");
            if (numbers.Count != 5) throw new ArgumentException("numbers", "There cannot be more than 5 numbers.");

            // Values based on October 2017 rules change.
            if (numbers.Any(n => (n < 1 || n > 70))) throw new ArgumentException("numbers", "At least one number is out of range (valid range: 1-70 (inclusive)).");
            //if (numbers.Any(n => (n < 1 || n > 56))) throw new ArgumentException("numbers", "At least one number is out of range (valid range: 1-56 (inclusive)).");

            if (megaplier < 1 || megaplier > 25) throw new ArgumentException("megaplier", "megaplier is out of range (valid range: 1-46 (inclusive)).");
            //if (megaplier < 1 || megaplier > 46) throw new ArgumentException("megaplier", "megaplier is out of range (valid range: 1-46 (inclusive)).");

            Numbers = numbers;
            Megaplier = megaplier;
        }

        public List<int> Numbers
        {
            get; private set;
        }

        public int Megaplier
        {
            get; private set;
        }
    }
}
