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
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            WebRequest req = WebRequest.Create(Url);
            var stream = req.GetResponse().GetResponseStream();
            stopwatch.Stop();
            Console.WriteLine($"Time to reach url: {stopwatch.ElapsedMilliseconds / 1000.0}");

            using (StreamReader r = new StreamReader(stream))
            {
                return r.ReadToEnd();
            }
        }
    }
}
