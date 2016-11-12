using System;
using System.IO;
using System.Net;
using System.Text;

namespace Debugger
{
    internal class WebCaller
    {
        internal static WebCaller GetInstance(Parser p)
        {
            return new WebCaller(p);
        }

        private Parser parser;

        private WebCaller(Parser p)
        {
            parser = p;
        }

        internal void GetUrl(string url)
        {
            Console.WriteLine($"Calling:{Environment.NewLine}{url}{Environment.NewLine}");

            WebRequest wr = WebRequest.Create(url);
            Stream objStream = wr.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            Console.WriteLine($"Recieved:{Environment.NewLine}{parser.Parse(objReader.ReadToEnd())}{Environment.NewLine}");
        }

        internal void PostUrl(string url, string postData)
        {
            Console.WriteLine($"Calling:{Environment.NewLine}{url} --> {postData}{Environment.NewLine}");

            var req = (HttpWebRequest)WebRequest.Create(url);

            var data = Encoding.ASCII.GetBytes(postData);

            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = data.Length;

            using (var stream = req.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var resp = (HttpWebResponse)req.GetResponse();
            var responseString = new StreamReader(resp.GetResponseStream()).ReadToEnd();

            Console.WriteLine($"Recieved:{Environment.NewLine}{parser.Parse(responseString)}{Environment.NewLine}");
        }
    }
}