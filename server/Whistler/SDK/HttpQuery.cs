using System.Collections.Generic;
using Whistler.SDK.Models;

namespace Whistler.SDK
{
    class HttpQuery
    {
        public static string GET(string Url, List<HttpQueryParam> Data)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url + "?" + string.Join("&", Data));
            req.Timeout = 10000;
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }
    }
}
