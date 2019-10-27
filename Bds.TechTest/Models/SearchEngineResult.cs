using System;
using System.Collections.Generic;
using System.Linq;

namespace Bds.TechTest.Models
{
    public class SearchEngineResult
    {
        public string Title { get; }
        public Uri Uri { get; }

        [NonSerialized]
        public Dictionary<string, int> Ranks = new Dictionary<string, int>();

        public IList<Tuple<string, int>> DisplayedRanks
        {
            get
            {
                return Ranks.Select(r => new Tuple<string, int>(r.Key, r.Value)).ToList();
            }
        }

        public SearchEngineResult(string title, Uri uri, string engineName, int rank)
        {
            Title = title;
            Uri = uri;
            Ranks[engineName] = rank;
        }

        public override int GetHashCode()
        {
            return Uri.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return ((SearchEngineResult)obj).Uri == Uri;
        }
    }
}
