using Bds.TechTest.Models;
using Moq;
using NUnit.Framework;

namespace Bds.TechTest.Tests
{
    public class DuckDuckGoEngineTest
    {
        private DuckDuckGoEngine _duckDuckGoEngine;

        [SetUp]
        public void Setup()
        {
            _duckDuckGoEngine = new DuckDuckGoEngine();
        }

        [Test]
        public void TestCreation()
        {
            Assert.That(_duckDuckGoEngine.Name == "DuckDuckGo");
        }

        [Test]
        public void TestCorrectQuery()
        {
            // Test by passing in a Moq of the HTTP runner.

            // Use the Moq to check that it is forming the query the way we expect

            string resultsHtml = "";

            var mockQueryRunner = new Mock<IQueryRunner>();

            mockQueryRunner.Setup(qr => qr.RunQuery(It.Is<string>(s => s == "https://duckduckgo.com/html/?q=Joss"))).Returns(resultsHtml).Verifiable();

            var results = _duckDuckGoEngine.SearchFor("Joss", mockQueryRunner.Object);

            mockQueryRunner.Verify();
        }

        [Test]
        public void TestResultParsing()
        {
            // Test by passing in a Moq that returns a page that the engine parses and check that the results come back as we expect

            // Typical Google test results for the term "Joss" were captured using:
            // $ curl -A "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"  https://www.google.com/search?q=Joss > googleresults.html
            // The user agent needs to not be curl otherwise Google will return an error and needs to appear to be a real browser otherwise the results are a bit mangled

            string resultsHtml = System.IO.File.ReadAllText("duckduckgoresults.html");

            var mockQueryRunner = new Mock<IQueryRunner>();

            mockQueryRunner.Setup(qr => qr.RunQuery(It.IsAny<string>())).Returns(resultsHtml);

            var results = _duckDuckGoEngine.SearchFor("", mockQueryRunner.Object);

            // Check the results - duckduckgoresults.html has 9 real results if we exclude the annoying suggestions and advertising;
            Assert.That(results.Count == 26);

            Assert.That(results[0].Ranks[_duckDuckGoEngine.Name] == 0);
            Assert.That(results[0].Title == "Beautiful Home Decor, Beautifully Priced");
            Assert.That(results[0].Uri.AbsoluteUri == "https://www.jossandmain.com/");

            Assert.That(results[1].Ranks[_duckDuckGoEngine.Name] == 1);
            Assert.That(results[1].Title == "JOSS - Wikipedia");
            Assert.That(results[1].Uri.AbsoluteUri == "https://en.wikipedia.org/wiki/JOSS");

            Assert.That(results[2].Ranks[_duckDuckGoEngine.Name] == 2);
            Assert.That(results[2].Title == "Joss Whedon - Wikipedia");
            Assert.That(results[2].Uri.AbsoluteUri == "https://en.wikipedia.org/wiki/Joss_Whedon");

            Assert.That(results[3].Ranks[_duckDuckGoEngine.Name] == 3);
            Assert.That(results[3].Title == "swimjoss.ru");
            Assert.That(results[3].Uri.AbsoluteUri == "http://www.swimjoss.ru/");

            Assert.That(results[4].Ranks[_duckDuckGoEngine.Name] == 4);
            Assert.That(results[4].Title == "Joss Whedon (@joss) | Твиттер");
            Assert.That(results[4].Uri.AbsoluteUri == "https://twitter.com/joss");
        }
    }
}
