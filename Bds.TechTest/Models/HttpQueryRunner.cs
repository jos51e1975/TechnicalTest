using System;

namespace Bds.TechTest.Models
{
    public class HttpQueryRunner : IQueryRunner
    {
        public string RunQuery(string query)
        {
            return System.IO.File.ReadAllText("Models/googleresults.html");
        }
    }
}
