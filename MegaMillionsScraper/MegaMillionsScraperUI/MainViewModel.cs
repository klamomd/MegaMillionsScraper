using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using MegaMillionsScraper;

namespace MegaMillionsScraperUI
{
    public class MainViewModel : BindableBase
    {
        // CONSTRUCTOR
        public MainViewModel()
        {
            _startDate = new DateTime(2017, 10, 31);
            _endDate = DateTime.Today;

#if DEBUG
            _scrapedNumbers.Add(new MegaMillionsNumbers(new DateTime(2017, 10, 31), new List<int> { 1, 2, 3, 4, 5 }, 1));
            _scrapedNumbers.Add(new MegaMillionsNumbers(new DateTime(2017, 11, 1), new List<int> { 6, 7, 8, 9, 10 }, 2));
            _scrapedNumbers.Add(new MegaMillionsNumbers(new DateTime(2017, 11, 2), new List<int> { 11, 12, 13, 14, 15 }, 3));
            _scrapedNumbers.Add(new MegaMillionsNumbers(new DateTime(2017, 11, 3), new List<int> { 16, 17, 18, 19, 20 }, 4));
            _scrapedNumbers.Add(new MegaMillionsNumbers(new DateTime(2017, 11, 4), new List<int> { 21, 22, 23, 24, 25 }, 5));

            _whiteBallOccurrences.Add(1, 2);
            _whiteBallOccurrences.Add(2, 4);
            _whiteBallOccurrences.Add(3, 6);
            _whiteBallOccurrences.Add(4, 8);
            _whiteBallOccurrences.Add(5, 10);

            _megaBallOccurrences.Add(1, 3);
            _megaBallOccurrences.Add(2, 6);
            _megaBallOccurrences.Add(3, 9);
#endif
        }

        // VARIABLES
        private DateTime _startDate;
        private DateTime _endDate;
        private List<MegaMillionsNumbers> _scrapedNumbers = new List<MegaMillionsNumbers>();
        private Dictionary<int, int> _whiteBallOccurrences = new Dictionary<int, int>();
        private Dictionary<int, int> _megaBallOccurrences = new Dictionary<int, int>();


        // PROPERTIES
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                if (value != _startDate)
                {
                    _startDate = value;
                    RaisePropertyChanged("StartDate");
                }
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (value != _endDate)
                {
                    _endDate = value;
                    RaisePropertyChanged("EndDate");
                }
            }
        }

        public List<MegaMillionsNumbers> ScrapedNumbers
        {
            get { return _scrapedNumbers; }
            set
            {
                if (value != _scrapedNumbers)
                {
                    _scrapedNumbers = value;
                    RaisePropertyChanged("ScrapedNumbers");
                }
            }
        }

        public Dictionary<int, int> WhiteBallOccurrences
        {
            get { return _whiteBallOccurrences; }
            set
            {
                if (value != _whiteBallOccurrences)
                {
                    _whiteBallOccurrences = value;
                    RaisePropertyChanged("WhiteBallOccurrences");
                }
            }
        }

        public Dictionary<int, int> MegaBallOccurrences
        {
            get { return _megaBallOccurrences; }
            set
            {
                if (value != _megaBallOccurrences)
                {
                    _megaBallOccurrences = value;
                    RaisePropertyChanged("MegaBallOccurrences");
                }
            }
        }
    }
}
