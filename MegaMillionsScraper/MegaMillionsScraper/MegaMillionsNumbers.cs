using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillionsScraper
{
    // A class used to contain a set of MegaMillions numbers.
    public class MegaMillionsNumbers
    {
        public MegaMillionsNumbers(DateTime dateOfDrawing, List<int> numbers, int megaplier)
        {
            if (dateOfDrawing == null) throw new ArgumentNullException("dateOfDrawing");

            if (numbers == null) throw new ArgumentNullException("numbers", "numbers cannot be null");
            if (numbers.Count != 5) throw new ArgumentException("numbers", "There cannot be more than 5 numbers.");

            // Values based on October 2017 rules change.
            if (numbers.Any(n => (n < 1 || n > 70))) throw new ArgumentException("numbers", "At least one number is out of range (valid range: 1-70 (inclusive)).");
            //if (numbers.Any(n => (n < 1 || n > 56))) throw new ArgumentException("numbers", "At least one number is out of range (valid range: 1-56 (inclusive)).");

            if (megaplier < 1 || megaplier > 25) throw new ArgumentException("megaplier", "megaplier is out of range (valid range: 1-25 (inclusive)).");
            //if (megaplier < 1 || megaplier > 46) throw new ArgumentException("megaplier", "megaplier is out of range (valid range: 1-46 (inclusive)).");

            DateOfDrawing = dateOfDrawing;
            Numbers = numbers;
            Megaplier = megaplier;
        }

        public DateTime DateOfDrawing
        {
            get; private set;
        }

        public List<int> Numbers
        {
            get; private set;
        }

        public int Megaplier
        {
            get; private set;
        }

        public string NumbersString
        {
            get
            {
                return string.Join(" ", Numbers);
            }
        }
    }
}
