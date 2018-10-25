using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MegaMillionsScraper
{

    public static class WebpageScraper
    {
        // Base URL to use for assembling the winning numbers page. I would have used the page that returns a range of dates all in one table, but the numbers themselves don't actually show up
        // in the HTML source because they are fetched by JavaScript code running an SQL query. I wasn't sure that there'd be a clean way to pull that info out, but noticed that on the individual
        // drawing results pages, the numbers show up in the HTML source, so I use that instead.
        const string baseMegaMillionsURL = "http://www.megamillions.com/winning-numbers/";

        // Given a DateTime, assembles the URL to the individual drawing page for that date.
        public static string GetMegaMillionsUrlForDate(DateTime date)
        {
            if (date == null) throw new ArgumentNullException("date");

            StringBuilder newUrl = new StringBuilder(baseMegaMillionsURL);
            newUrl.AppendFormat("{0}-{1}-{2}", date.Month, date.Day, date.Year);

            return newUrl.ToString();
        }

        // Using HTML Agility Pack, fetches the web page contents and scrapes out the data located in the white ball and mega ball divs. Then it returns that info as a MegaMillionsNumbers.
        public static MegaMillionsNumbers GetNumbersForDate(DateTime date)
        {
            if (date == null) throw new ArgumentNullException("date");

            string url = GetMegaMillionsUrlForDate(date);
            var web = new HtmlWeb();
            var doc = web.Load(url);

            // Pull the divs with the appropriate class.
            var findWhiteBallClasses = doc.DocumentNode.Descendants("div").Where(d => d.HasClass("winning-numbers-white-ball"));
            var findMegaBallClasses = doc.DocumentNode.Descendants("div").Where(d => d.HasClass("winning-numbers-mega-ball"));

            // Must be 5 white balls and 1 mega ball.
            if (findWhiteBallClasses.Count() != 5) throw new Exception(string.Format("Invalid number of white ball divs! ({0})", findWhiteBallClasses.Count()));
            if (findMegaBallClasses.Count() != 1) throw new Exception(string.Format("Invalid number of mega ball divs! ({0})", findMegaBallClasses.Count()));

            // Parse out the white balls.
            List<int> whiteBalls = new List<int>();
            foreach (var div in findWhiteBallClasses)
            {
                int w = 0;
                if (int.TryParse(div.InnerText.Trim(), out w))
                {
                    whiteBalls.Add(w);
                }
                else throw new Exception("Failed to parse the contents of one of the white ball divs.");
            }

            // Parse out the mega balls.
            int megaBall = -1;
            if (!int.TryParse(findMegaBallClasses.First().InnerText.Trim(), out megaBall))
            {
                throw new Exception("Failed to parse the contents of the mega ball div.");
            }

            return new MegaMillionsNumbers(date, whiteBalls, megaBall);
        }

        public static async Task<MegaMillionsNumbers> GetNumbersForDateAsync(DateTime date)
        {
            return await Task.Run(() => GetNumbersForDate(date));
        }
    }
}
