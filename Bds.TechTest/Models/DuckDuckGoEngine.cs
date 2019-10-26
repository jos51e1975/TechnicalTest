using System;
using System.Collections.Generic;

namespace Bds.TechTest.Models
{
    public class DuckDuckGoEngine : SearchEngine
    {
        public DuckDuckGoEngine() : base("DuckDuckGo")
        {
        }

        public override IList<SearchEngineResult> SearchFor(string queryTerm, IQueryRunner queryRunner)
        {
            throw new NotImplementedException();
        }
    }
}
