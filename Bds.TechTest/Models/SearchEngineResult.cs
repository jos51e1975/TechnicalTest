using System;
namespace Bds.TechTest.Models
{
    public class SearchEngineResult
    {
        public string Title { get; }
        public Uri Uri { get; }

        public SearchEngineResult(string title, Uri uri)
        {
            Title = title;
            Uri = uri;
        }
    }
}
