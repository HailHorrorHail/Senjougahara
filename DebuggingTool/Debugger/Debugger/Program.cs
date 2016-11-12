using System;

namespace Debugger
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunGetRequests();
            RunPostRequests();

            Console.ReadKey();
        }

        private static void RunPostRequests()
        {
            var parser = Parser.GetInstance();
            var wc = WebCaller.GetInstance(parser);

            wc.PostUrl("http://localhost:61418/Events", "Title=Test&Description=SomeDescription");
        }

        private static void RunGetRequests()
        {
            var parser = Parser.GetInstance();
            var wc = WebCaller.GetInstance(parser);

            wc.GetUrl(@"http://localhost:61418/Events");
            wc.GetUrl(@"http://localhost:61418/Events?Id=1");
        }
    }
}
