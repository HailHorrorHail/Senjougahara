using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            Console.WriteLine($"------------>Calling<------------{Environment.NewLine}{url}{Environment.NewLine}");

            WebRequest wr = WebRequest.Create(url);
            Stream objStream = wr.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            Console.WriteLine($"Recieved:{Environment.NewLine}{parser.Parse(objReader.ReadToEnd())}{Environment.NewLine}");
        }

        internal void PostUrl(string url, string method, params string[] postData)
        {
            Task t = PostUrlAsync(url, method, postData);
            t.Wait();
        }

        internal async Task PostUrlAsync(string url, string method, params string[] postData)
        {
            string postDataSingle = string.Join("&", postData);
            Console.WriteLine($"------------>Calling<------------{Environment.NewLine}{url} --> {method} --> {postDataSingle}{Environment.NewLine}");

            string responseString, reason;
            HttpRequestMessage retRequestMsg;
            HttpStatusCode statusCode;
            using (var client = new HttpClient())
            {
                Dictionary<string, string> values = postData.ToDictionary(d => d.Split('=')[0], d => d.Split('=')[1]);

                var content = new FormUrlEncodedContent(values);

                var response = await client.PostAsync(url, content);
                reason = response.ReasonPhrase;
                statusCode = response.StatusCode;

                retRequestMsg = response.RequestMessage;

                responseString = await response.Content.ReadAsStringAsync();
            }

            Console.WriteLine($"Returned Request:{Environment.NewLine}{retRequestMsg.ToString()}{Environment.NewLine}");
            Console.WriteLine($"Recieved (Reason:{reason} Status:{statusCode}):{Environment.NewLine}{parser.Parse(responseString)}{Environment.NewLine}");
        }

        internal void PostUrl_Old(string url, string method, params string[] postData)
        {
            string postDataSingle = string.Join("&", postData);
            Console.WriteLine($"------------>Calling<------------{Environment.NewLine}{url} --> {method} --> {postDataSingle}{Environment.NewLine}");

            var req = (HttpWebRequest)WebRequest.Create(url);

            var data = Encoding.ASCII.GetBytes(postDataSingle);

            req.Method = method;
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