using NUnit.Framework;
using Moq;

using Bds.TechTest.Models;

namespace Bds.TechTest.Tests
{
    public class GoogleEngineTests
    {
        private GoogleEngine _googleEngine;

        [SetUp]
        public void Setup()
        {
            _googleEngine = new GoogleEngine();
        }

        [Test]
        public void TestCreation()
        {
            Assert.That(_googleEngine.Name == "Google");
        }

        [Test]
        public void TestCorrectQuery()
        {
            // Test by passing in a Moq of the HTTP runner.

            // TODO: Should the runner be threaded? Probably - use async...await

            // Use the Moq to check that it is forming the query the way we expect

            string resultsHtml = "";

            var mockQueryRunner = new Mock<IQueryRunner>();

            mockQueryRunner.Setup(qr => qr.RunQuery(It.Is<string>(s => s == "https://www.google.com/search?q=Joss"))).Returns(resultsHtml).Verifiable();

            var results = _googleEngine.SearchFor("Joss", mockQueryRunner.Object);

            mockQueryRunner.Verify();
        }

        [Test]
        public void TestResultParsing()
        {
            // Test by passing in a Moq that returns a page that the engine parses and check that the results come back as we expect

            // Typical Google test results for the term "Joss" were captured using:
            // $ curl -A "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36"  https://www.google.com/search?q=Joss > googleresults.html
            // The user agent needs to not be curl otherwise Google will return an error and needs to appear to be a real browser otherwise the results are a bit mangled

            string resultsHtml = System.IO.File.ReadAllText("googleresults.html");

            var mockQueryRunner = new Mock<IQueryRunner>();

            mockQueryRunner.Setup(qr => qr.RunQuery(It.IsAny<string>())).Returns(resultsHtml);

            var results = _googleEngine.SearchFor("", mockQueryRunner.Object);

            // Check the results - googleresults.html has 9 real results if we exclude the annoying google suggestions and advertising;

            // 1. Wiktionary
            // 2. jossstone.com
            // 3. urban dictionary
            // 4. Merriam-Webster
            // 5. Wikipedia (Chinese statue)
            // 6. Wikipedia (acronym)
            // 7. Dictionary.com
            // 8. Twitter - Joss Whedon
            // 9. Lexico

            Assert.That(results.Count == 9);

            Assert.That(results[0].Ranks[_googleEngine.Name] == 0);
            Assert.That(results[0].Title == "joss - Wiktionary");
            Assert.That(results[0].Uri.AbsoluteUri == "https://en.wiktionary.org/wiki/joss");

            Assert.That(results[1].Ranks[_googleEngine.Name] == 1);
            Assert.That(results[1].Title == "Joss Stone :: Joss Stone | Project Mama Earth");
            Assert.That(results[1].Uri.AbsoluteUri == "https://www.jossstone.com/");

            Assert.That(results[2].Ranks[_googleEngine.Name] == 2);
            Assert.That(results[2].Title == "Joss - Urban Dictionary");
            Assert.That(results[2].Uri.AbsoluteUri == "https://www.urbandictionary.com/define.php?term=Joss");

            Assert.That(results[3].Ranks[_googleEngine.Name] == 3);
            Assert.That(results[3].Title == "Joss | Definition of Joss by Merriam-Webster");
            Assert.That(results[3].Uri.AbsoluteUri == "https://www.merriam-webster.com/dictionary/joss");

            Assert.That(results[4].Ranks[_googleEngine.Name] == 4);
            Assert.That(results[4].Title == "Joss (Chinese Statue) - Wikipedia");
            Assert.That(results[4].Uri.AbsoluteUri == "https://en.wikipedia.org/wiki/Joss_(Chinese_Statue)");

            Assert.That(results[5].Ranks[_googleEngine.Name] == 5);
            Assert.That(results[5].Title == "JOSS - Wikipedia");
            Assert.That(results[5].Uri.AbsoluteUri == "https://en.wikipedia.org/wiki/JOSS");

            Assert.That(results[6].Ranks[_googleEngine.Name] == 6);
            Assert.That(results[6].Title == "Joss | Definition of Joss at Dictionary.com");
            Assert.That(results[6].Uri.AbsoluteUri == "https://www.dictionary.com/browse/joss");

            Assert.That(results[7].Ranks[_googleEngine.Name] == 7);
            Assert.That(results[7].Title == "Joss Whedon (@joss) | Twitter");
            Assert.That(results[7].Uri.AbsoluteUri == "https://twitter.com/joss?lang=en");

            Assert.That(results[8].Ranks[_googleEngine.Name] == 8);
            Assert.That(results[8].Title == "Joss | Definition of Joss by Lexico");
            Assert.That(results[8].Uri.AbsoluteUri == "https://www.lexico.com/en/definition/joss");
        }
    }
}