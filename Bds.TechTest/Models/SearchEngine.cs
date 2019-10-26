using System;
using System.Collections.Generic;

namespace Bds.TechTest.Models
{
    public abstract class SearchEngine
    {
        public string Name { get; protected set;  }

        public abstract IList<SearchEngineResult> SearchFor(string queryTerm, IQueryRunner queryRunner);

        protected SearchEngine(string name)
        {
            Name = name;
        }
    }
}
