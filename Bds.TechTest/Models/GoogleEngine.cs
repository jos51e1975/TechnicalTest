using System;
using System.Collections.Generic;

namespace Bds.TechTest.Models
{
    public class GoogleEngine : SearchEngine
    {
        public GoogleEngine() : base("Google")
        {
        }

        public override IEnumerable<SearchEngineResult> RunQuery(string queryTerm)
        {
            throw new NotImplementedException();
        }
    }
}
