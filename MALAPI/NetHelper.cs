using System;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Diagnostics;

namespace MALAPI
{
    class NetHelper
    {
        public static string Get(string Url)
        {
            WebRequest req = WebRequest.Create(Url);
            var stream = req.GetResponse().GetResponseStream();

            using (StreamReader r = new StreamReader(stream))
            {
                return r.ReadToEnd();
            }
        }
    }
}
