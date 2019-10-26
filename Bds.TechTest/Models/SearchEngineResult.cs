using System;

namespace Bds.TechTest.Models
{
    public class SearchEngineResult
    {
        public string Title { get; }
        public Uri Uri { get; }
        public int Rank { get;  }

        public SearchEngineResult(string title, Uri uri, int rank)
        {
            Title = title;
            Uri = uri;
            Rank = rank;
        }
    }
}
