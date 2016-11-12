using System;
using System.Collections.Generic;
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
    }
}