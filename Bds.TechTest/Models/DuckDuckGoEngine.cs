using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace Bds.TechTest.Models
{
    public class DuckDuckGoEngine : SearchEngine
    {
        public DuckDuckGoEngine() : base("DuckDuckGo")
        {
        }

        public override IList<SearchEngineResult> SearchFor(string queryTerm, IQueryRunner queryRunner)
        {
            // Create the query...

            string query = $"https://duckduckgo.com/html/?q={queryTerm}";

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

                var resultNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='result results_links results_links_deep web-result ']");

                foreach (var node in resultNodes)
                {
                    var resultA = node.SelectSingleNode(".//a[@class='result__a']");

                    var uriString = resultA.Attributes["href"].Value;

                    var title = resultA.InnerText;

                    results.Add(new SearchEngineResult(title, new Uri(uriString), Name, rank));

                    rank++;
                }
            }
            catch (Exception ex)
            {
                // TODO!!
                //Debug.Write(ex.ToString());
            }

            return results;
        }
    }
}
