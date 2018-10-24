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
        const string baseMegaMillionsURL = "http://www.megamillions.com/winning-numbers/";

        public static string GetMegaMillionsUrlForDate(DateTime date)
        {
            if (date == null) throw new ArgumentNullException("date");

            StringBuilder newUrl = new StringBuilder(baseMegaMillionsURL);
            newUrl.AppendFormat("{0}-{1}-{2}", date.Month, date.Day, date.Year);

            return newUrl.ToString();
        }

        public static MegaMillionsNumbers GetNumbersForDate(DateTime date)
        {
            if (date == null) throw new ArgumentNullException("date");

            string url = GetMegaMillionsUrlForDate(date);
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var findWhiteBallClasses = doc.DocumentNode.Descendants("div").Where(d => d.HasClass("winning-numbers-white-ball"));
            var findMegaBallClasses = doc.DocumentNode.Descendants("div").Where(d => d.HasClass("winning-numbers-mega-ball"));

            if (findWhiteBallClasses.Count() != 5) throw new Exception(string.Format("Invalid number of white ball divs! ({0})", findWhiteBallClasses.Count()));
            if (findMegaBallClasses.Count() != 1) throw new Exception(string.Format("Invalid number of mega ball divs! ({0})", findMegaBallClasses.Count()));


            List<int> whiteBalls = new List<int>();
            int megaBall = -1;
            foreach (var div in findWhiteBallClasses)
            {
                int w = 0;
                if (int.TryParse(div.InnerText.Trim(), out w))
                {
                    whiteBalls.Add(w);
                }
                else throw new Exception("Failed to parse the contents of one of the white ball divs.");
            }

            if (!int.TryParse(findMegaBallClasses.First().InnerText.Trim(), out megaBall))
            {
                throw new Exception("Failed to parse the contents of the mega ball div.");
            }

            return new MegaMillionsNumbers(whiteBalls, megaBall);
        }
    }
}
