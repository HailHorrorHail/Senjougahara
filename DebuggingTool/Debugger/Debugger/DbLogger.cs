using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Debugger
{
    class DbLogger
    {
        public static DbLogger Write = new DbLogger();


        public void Information(string str, params object[] objs)
        {
            Console.WriteLine(str, objs);        
        }

        public void Error(string str, params object[] objs)
        {
            Console.WriteLine(str, objs);
        }

        public void Verbose(string str, params object[] objs)
        {
            //Console.WriteLine(str, objs);
        }
    }
}
