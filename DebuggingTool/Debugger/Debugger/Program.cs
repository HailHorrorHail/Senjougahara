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

            wc.GetUrl(@"http://localhost:61418/Events");

            wc.PostUrl("http://localhost:61418/Events", "POST", "Title=Test", "Description=SomeDescription");

            wc.PostUrl("http://localhost:61418/Events(1)", "UPDATE", "Id=1", "Title=Test.Update");
            wc.GetUrl(@"http://localhost:61418/Events?Id=1");

            wc.PostUrl("http://localhost:61418/Events(1)", "DELETE");
            wc.GetUrl(@"http://localhost:61418/Events?Id=1");
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
