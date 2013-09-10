
using System;
using System.Linq;
using System.Threading;
using Nancy.Hosting.Self;
using log4net;
using log4net.Config;

namespace DeReader
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var uri = "http://localhost:8888";
                Console.WriteLine(uri);

                XmlConfigurator.Configure();

                ILog logger = LogManager.GetLogger(typeof(Program));
                logger.InfoFormat("Starting DeReader : {0}", uri);

                // initialize an instance of NancyHost (found in the Nancy.Hosting.Self package)
                var host = new NancyHost(new Uri(uri));
                host.Start();  // start hosting

                //Under mono if you deamonize a process a Console.ReadLine with cause an EOF 
                //so we need to block another way
                if (args.Any(s => s.Equals("-d", StringComparison.CurrentCultureIgnoreCase)))
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                else
                {
                    Console.ReadKey();
                }

                logger.Info("Stoping DeReader");
                host.Stop();  // stop hosting
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
    }
}
