using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bds.TechTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DoSearch(string searchTerm)
        {
            try
            {
                _logger.LogDebug($"Searching for {searchTerm}");

                // We could expand this to allow more engines and even show tick boxes on screen
                // for which ones we want to use.

                // TODO: Use MEF or reflection to get the search engines

                var engines = new List<SearchEngine> { new GoogleEngine() , new DuckDuckGoEngine() };

                var queryRunner = new HttpQueryRunner();

                var combinedResults = RunQueryThroughEngines(searchTerm, engines, queryRunner);

                _logger.LogDebug($"Found {combinedResults.Count} results for {searchTerm}");

                var engineNamesAndResults = new Tuple<List<string>, HashSet<SearchEngineResult>>(engines.Select(e => e.Name).ToList(), combinedResults);

                var json = Json(engineNamesAndResults);

                return json;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to get results from engines");
            }

            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Shouldn't be public - needed for tests; describe how it should be done with the whole
        // HomeController DoSearch and Dependency Injection
        public HashSet<SearchEngineResult> RunQueryThroughEngines(string searchTerm, List<SearchEngine> engines, IQueryRunner queryRunner)
        {
            // The results are merged from all the engines, with rankings for each engine
            // where the index of the rank in the list is the index of the engine in the engines list
            var allResults = new HashSet<SearchEngineResult>();

            foreach (var engine in engines)
            {

                var results = engine.SearchFor(searchTerm, queryRunner);

                foreach (var result in results)
                {
                    if (allResults.TryGetValue(result, out var foundResult))
                    {
                        foundResult.Ranks[result.Ranks.First().Key] = result.Ranks.First().Value;
                    }
                    else
                    {
                        allResults.Add(result);
                    }
                }
            }

            return allResults;
        }
    }
}
