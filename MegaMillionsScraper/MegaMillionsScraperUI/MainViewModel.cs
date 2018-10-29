using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using MegaMillionsScraper;
using System.IO;
using Microsoft.Win32;

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
            _browseForResultsFilePathCommand = new DelegateCommand(BrowseForResultsFilePath, CanBrowseForResultsFilePath);
            _resultsFilePath = new FileInfo(@"./Results.txt").FullName;

            _resultsFileSaveDialog = new SaveFileDialog();
            _resultsFileSaveDialog.FileName = "Results";
            _resultsFileSaveDialog.DefaultExt = ".txt";
            _resultsFileSaveDialog.Filter = "Text documents (.txt)|*.txt";

        }

        // VARIABLES
        private DateTime _startDate;
        private DateTime _endDate;

        private List<MegaMillionsNumbers> _scrapedNumbers = new List<MegaMillionsNumbers>();

        private Dictionary<int, int> _whiteBallOccurrences = new Dictionary<int, int>();
        private Dictionary<int, int> _megaBallOccurrences = new Dictionary<int, int>();

        private DelegateCommand _scrapeNumbersCommand;
        private DelegateCommand _browseForResultsFilePathCommand;

        private bool _isScraping = false;

        private int _currentProgress = 0;
        private int _progressMaximum = 100;

        private bool _writeResultsToFile = false;
        private string _resultsFilePath;

        private SaveFileDialog _resultsFileSaveDialog;

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

        public DelegateCommand BrowseForResultsFilePathCommand
        {
            get { return _browseForResultsFilePathCommand; }
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
                    RaisePropertyChanged("AreControlsEnabled");
                    RaisePropertyChanged("ScrapeButtonText");
                    ScrapeNumbersCommand.RaiseCanExecuteChanged();
                    BrowseForResultsFilePathCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public bool AreControlsEnabled
        {
            get { return !IsScraping; }
        }

        public string ScrapeButtonText
        {
            get
            {
                if (IsScraping) return "Scraping...";
                else return "Scrape Numbers";
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

        public bool WriteResultsToFile
        {
            get { return _writeResultsToFile; }
            set
            {
                if (value != _writeResultsToFile)
                {
                    _writeResultsToFile = value;
                    RaisePropertyChanged("WriteResultsToFile");
                }
            }
        }

        public string ResultsFilePath
        {
            get { return _resultsFilePath; }
            set
            {
                if (value != _resultsFilePath)
                {
                    _resultsFilePath = value;
                    RaisePropertyChanged("ResultsFilePath");
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

            // Write contents to file if specified.
            if (WriteResultsToFile)
            {
                CreateResultsFile(ResultsFilePath);
            }

            IsScraping = false;
        }

        public bool CanScrapeNumbers()
        {
            return !IsScraping;
        }

        public void BrowseForResultsFilePath()
        {
            Nullable<bool> result = _resultsFileSaveDialog.ShowDialog();

            if (result == true)
            {
                ResultsFilePath = _resultsFileSaveDialog.FileName;
            }
        }

        public bool CanBrowseForResultsFilePath()
        {
            return !IsScraping;
        }

        // Creates a text file at the given path which contains the white and mega ball occurrences (ordered by number), and then white and mega ball occurrences (ordered by occurrences).
        public void CreateResultsFile(string filePath)
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine("White ball occurrences:");
                for (int i = 1; i <= WhiteBallOccurrences.Keys.Max(); i++)
                {
                    file.WriteLine(string.Format("{0}: {1}", i, WhiteBallOccurrences[i]));
                }

                file.WriteLine();

                file.WriteLine("Mega ball occurrences:");
                for (int i = 1; i <= MegaBallOccurrences.Keys.Max(); i++)
                {
                    file.WriteLine(string.Format("{0}: {1}", i, MegaBallOccurrences[i]));
                }
                file.WriteLine();

                // Order the number of occurrences for both types of numbers (for easier viewing in file).
                var orderedWhiteBallOccurrences = WhiteBallOccurrences.OrderByDescending(w => w.Value);
                var orderedMegaBallOccurrences = MegaBallOccurrences.OrderByDescending(w => w.Value);

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

                file.WriteLine("Raw number sets:");
                foreach (var numberSet in ScrapedNumbers)
                {
                    file.WriteLine(string.Format("{0}: {1} - {2}", numberSet.DateOfDrawingString, numberSet.NumbersString, numberSet.Megaplier));
                }
            }
        }
    }
}
