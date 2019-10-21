using System;
using System.Collections.Generic;

namespace Bds.TechTest.Models
{
    public abstract class SearchEngine
    {
        protected string Name { get; private set;  }

        public abstract IEnumerable<SearchEngineResult> RunQuery(string queryTerm);

        protected SearchEngine(string name)
        {
            Name = name;
        }
    }
}
