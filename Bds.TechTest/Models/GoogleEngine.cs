// As the test is to not use their API, we need to appear as if we are an ordinary browser
// and scrape the returned pages;
// Some useful information at: http://google-scraper.squabbel.com

using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Bds.TechTest.Models
{
    public class GoogleEngine : SearchEngine
    {
        public GoogleEngine() : base("Google")
        {
        }

        public override IList<SearchEngineResult> SearchFor(string queryTerm, IQueryRunner queryRunner)
        {
            // Create the query...

            string query = $"https://www.google.com/search?q={queryTerm}";

            // ...run the query through google...

            string returnedHtml = queryRunner.RunQuery(query);

            // ...parse the returned HTML to gather the results
            
            var results = new List<SearchEngineResult>();

            try
            {
                var htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(returnedHtml);

                // We need to look for div tags with class g

                int rank = 0;

                var resultNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='g']");

                foreach (var node in resultNodes)
                {
                    var resultDiv = node.SelectSingleNode(".//div[@class='r']");

                    var uriString = resultDiv.FirstChild.Attributes["href"].Value;

                    var title = resultDiv.SelectSingleNode(".//h3").InnerText;

                    results.Add(new SearchEngineResult(title, new Uri(uriString), Name, rank));

                    rank++;
                }
            }
            catch (Exception ex)
            {
                // TODO: It would be better 
                //Debug.Write(ex.ToString());
            }

            return results;
        }
    }
}
