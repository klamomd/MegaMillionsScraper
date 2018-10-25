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

            _scrapeNumbersCommand = new DelegateCommand(ScrapeNumbers, CanScrapeNumbers);
        }

        // VARIABLES
        private DateTime _startDate;
        private DateTime _endDate;

        private List<MegaMillionsNumbers> _scrapedNumbers = new List<MegaMillionsNumbers>();

        private Dictionary<int, int> _whiteBallOccurrences = new Dictionary<int, int>();
        private Dictionary<int, int> _megaBallOccurrences = new Dictionary<int, int>();

        private DelegateCommand _scrapeNumbersCommand;

        private bool _isScraping = false;

        private int _currentProgress = 0;
        private int _progressMaximum = 100;

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

        public DelegateCommand ScrapeNumbersCommand
        {
            get { return _scrapeNumbersCommand; }
        }

        public DateTime MaxDate
        {
            get { return DateTime.Today; }
        }

        public bool IsScraping
        {
            get { return _isScraping; }
            set
            {
                if (value != _isScraping)
                {
                    _isScraping = value;
                    RaisePropertyChanged("IsScraping");
                    ScrapeNumbersCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public int ProgressMaximum
        {
            get { return _progressMaximum; }
            set
            {
                if (value != _progressMaximum)
                {
                    _progressMaximum = value;
                    RaisePropertyChanged("ProgressMaximum");
                }
            }
        }

        public int CurrentProgress
        {
            get { return _currentProgress; }
            set
            {
                if (value != _currentProgress)
                {
                    _currentProgress = value;
                    RaisePropertyChanged("CurrentProgress");
                }
            }
        }


        // FUNCTIONS
        public void ClearAllFields()
        {
            ScrapedNumbers = new List<MegaMillionsNumbers>();
            WhiteBallOccurrences = new Dictionary<int, int>();
            MegaBallOccurrences = new Dictionary<int, int>();
        }

        public async void ScrapeNumbers()
        {
            // TODO: Make this async so that the UI doesn't freeze up while working?

            ClearAllFields();
            IsScraping = true;

            List<DateTime> lottoDates = DateFetcher.FetchDateTimesInRange(StartDate, EndDate);

            // Reset the progress bar.
            ProgressMaximum = lottoDates.Count;
            CurrentProgress = 0;

            List<MegaMillionsNumbers> megaNumbers = new List<MegaMillionsNumbers>();
            foreach (var dt in lottoDates)
            {
                MegaMillionsNumbers numbers = await WebpageScraper.GetNumbersForDateAsync(dt);
                megaNumbers.Add(numbers);
                CurrentProgress = CurrentProgress + 1;
            }

            ScrapedNumbers = megaNumbers;
            WhiteBallOccurrences = NumberCalculator.GetOccurrencesOfWhiteBalls(megaNumbers);
            MegaBallOccurrences = NumberCalculator.GetOccurrencesOfMegaBalls(megaNumbers);

            IsScraping = false;
        }

        public bool CanScrapeNumbers()
        {
            return !IsScraping;
        }
    }
}
