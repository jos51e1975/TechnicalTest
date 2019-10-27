using System;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

using Test.Controllers;
using Bds.TechTest.Models;
using System.Collections.Generic;

namespace Bds.TechTest.Tests
{
    public class HomeControllerTest
    {
        private HomeController _homeController;

        private Mock<ILogger<HomeController>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<HomeController>>();

            _homeController = new HomeController(_logger.Object);
        }

        [Test]
        public void TestDoSearch()
        {
            var engines = new List<SearchEngine> { new GoogleEngine(), new DuckDuckGoEngine() };

            var mockQueryRunner = new Mock<IQueryRunner>();

            mockQueryRunner.Setup(qr => qr.RunQuery(It.IsAny<string>())).Returns(
                () =>
                {
                    string resultsHtml = "";

                    var engineType = GetCallingObjectType();

                    if (engineType == typeof(GoogleEngine))
                    {
                        resultsHtml = System.IO.File.ReadAllText("googleresults.html");
                    }
                    else if(engineType == typeof(DuckDuckGoEngine))
                    {
                        resultsHtml = System.IO.File.ReadAllText("duckduckgoresults.html");
                    }
                    else
                    {
                        Assert.Fail();
                    }

                    return resultsHtml;
                });

            var results = _homeController.RunQueryThroughEngines("Joss", engines, mockQueryRunner.Object);

            Assert.That(results.Count == 32); // Less than the 26 + 9; showing combined

            // Don't go through all of them as the individual engine tests do that!
        }

        // This is nasty and should only be used in Test code - NEVER USE THIS
        // ANYWHERE ELSE!!
        private Type GetCallingObjectType()
        {
            var stackTrace = new System.Diagnostics.StackTrace();

            // I'm worried this might have different depth on Mono, .NET or whatever
            return stackTrace.GetFrame(14).GetMethod().DeclaringType;
        }
    }
}
