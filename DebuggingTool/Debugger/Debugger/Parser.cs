using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Debugger
{
    internal class Parser
    {
        internal static Parser GetInstance()
        {
            var p = new Parser();
            
            return p;
        }

        private int IndentCount;
        private readonly Dictionary<char, Func<string>> Changes;

        private Parser()
        {
            IndentCount = 0;
            Changes = new Dictionary<char, Func<string>>
            {
                { '{', HandleOpenBrace },
                { '}', HandleClosedBrace },
                { ',', HandleComma }
            };
        }

        internal string Parse(string getResponse)
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in getResponse)
            {
                Func<string> toAddFunc;
                if (Changes.TryGetValue(c, out toAddFunc))
                {
                    sb.Append(toAddFunc());
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        private string HandleOpenBrace()
        {
            string toRet = $"{Environment.NewLine}{AddTabs(IndentCount)}{{{Environment.NewLine}{AddTabs(++IndentCount)}";

            return toRet;
        }

        private string HandleClosedBrace()
        {
            IndentCount--;
            return $"{Environment.NewLine}{AddTabs(IndentCount)}}}";
        }

        private string HandleComma()
        {
            return $"{Environment.NewLine}{AddTabs(IndentCount)},";
        }

        private static string AddTabs(int numTabs)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < numTabs; i++)
            {
                sb.Append("\t");
            }

            return sb.ToString();
        }

        internal string ParseError(string responseString)
        {
            StringBuilder sb = new StringBuilder();
            string[] strElements = responseString.Split(',');

            foreach (string str in strElements)
            {
                string[] values = str.Split(':');

                if (str.Contains("\"error\""))
                {
                    sb.Append("\tError:");
                }
                else if (str.Contains("\"innererror\""))
                {
                    sb.AppendLine();
                    sb.Append("\tInner Error:");
                }

                int loc = FindLocation(values, "\"code\"") + 1;
                if (loc > 0)
                {
                    sb.AppendFormat("Code:{0}", values[loc]);
                }

                loc = FindLocation(values, "\"message\"") + 1;
                if (loc > 0)
                {
                    sb.AppendFormat("Message:{0}", values[loc]);
                }
            }

            return sb.ToString();
        }

        private int FindLocation(string[] values, string target)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Contains(target))
                {
                    return i;
                }
            }

            return -50;
        }
    }
}