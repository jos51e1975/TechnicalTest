using System.Net;
using System.Text;

namespace Bds.TechTest.Models
{
    public class HttpQueryRunner : IQueryRunner
    {
        public string RunQuery(string query)
        {
            var request = (HttpWebRequest)WebRequest.Create(query);

            // Need to pretend to be Chrome so that we get appropriate responses back
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";

            var response = (HttpWebResponse)request.GetResponse();

            var sb = new StringBuilder();

            using (var resStream = response.GetResponseStream())
            {
                string tempString = null;

                int count = 0;

                var resultsBuffer = new byte[8192];

                do
                {
                    count = resStream.Read(resultsBuffer, 0, resultsBuffer.Length);

                    if (count != 0)
                    {
                        tempString = Encoding.ASCII.GetString(resultsBuffer, 0, count);

                        sb.Append(tempString);
                    }
                }
                while (count > 0);
            }

            return sb.ToString();
        }
    }
}
