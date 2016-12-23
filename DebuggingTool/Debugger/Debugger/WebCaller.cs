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
        private const string CallString = "----->   Calling {0}   <-----";
        private const string RetString = "-----< Recieved {0} >-----";

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
            DbLogger.Write.Information(CallString, url);

            WebRequest wr = WebRequest.Create(url);
            Stream objStream;
            string output = null;
            try
            {
                objStream = wr.GetResponse().GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);
                output = parser.Parse(objReader.ReadToEnd());
            }
            catch (WebException ex)
            {
                output = ex.Message;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            DbLogger.Write.Information(RetString, string.Empty);
            DbLogger.Write.Information("{0}{1}", output, Environment.NewLine);
        }

        internal void PostUrl(string url, string method, params string[] postData)
        {
            Task t = PostUrlAsync(url, method, postData);
            t.Wait();
        }

        internal async Task PostUrlAsync(string url, string method, params string[] postData)
        {
            try
            {
                string postDataSingle = string.Join("&", postData);
                DbLogger.Write.Information(CallString, url);
                DbLogger.Write.Information("\t{0} --> {1}", method, postDataSingle);

                string responseString, reason;
                HttpRequestMessage retRequestMsg;
                HttpStatusCode statusCode;
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = null;
                    Dictionary<string, string> values;
                    FormUrlEncodedContent content;
                    switch (method)
                    {
                        case "POST":
                            values = postData.ToDictionary(d => d.Split('=')[0], d => d.Split('=')[1]);
                            content = new FormUrlEncodedContent(values);

                            response = await client.PostAsync(url, content);
                            break;

                        case "DELETE":
                            response = await client.DeleteAsync(url);
                            break;

                        case "UPDATE":
                            values = postData.ToDictionary(d => d.Split('=')[0], d => d.Split('=')[1]);
                            content = new FormUrlEncodedContent(values);

                            client.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
                            client.DefaultRequestHeaders.Add("X-HTTP-Method", "MERGE");
                            client.DefaultRequestHeaders.Add("IF-MATCH", "*");
                            response = await client.PutAsync(url, content);
                            break;

                        default:
                            DbLogger.Write.Error("Unknown method:{0}", method);
                            break;
                    }
                    
                    reason = response.ReasonPhrase;
                    statusCode = response.StatusCode;

                    retRequestMsg = response.RequestMessage;

                    responseString = await response.Content.ReadAsStringAsync();
                }
                
                DbLogger.Write.Verbose("{1}Returned Request{1}{0}{1}", retRequestMsg.ToString(), Environment.NewLine);
                DbLogger.Write.Information(RetString, statusCode);
                if (statusCode != HttpStatusCode.OK)
                {
                    DbLogger.Write.Error("Status Code:{0} due to:{1}", statusCode, reason);
                    DbLogger.Write.Information(parser.ParseError(responseString));
                    DbLogger.Write.Verbose("{0}{1}", parser.Parse(responseString), Environment.NewLine);
                }
                else
                {
                    DbLogger.Write.Information("{0}{1}", parser.Parse(responseString), Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                DbLogger.Write.Error(ex.ToString());
            }

            DbLogger.Write.Information("");
        }
    }
}