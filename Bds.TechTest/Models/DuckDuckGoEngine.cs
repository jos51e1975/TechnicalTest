using System;
using System.Collections.Generic;

namespace Bds.TechTest.Models
{
    public class DuckDuckGoEngine : SearchEngine
    {
        public DuckDuckGoEngine() : base("DuckDuckGo")
        {
        }

        public override IEnumerable<SearchEngineResult> RunQuery(string queryTerm)
        {
            throw new NotImplementedException();
        }
    }
}
