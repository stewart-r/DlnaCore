using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace DlnaCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = BuildHost();

            host.Run();

        }

        public static void RunServerWithCancellation(CancellationToken cancellationTkn)
        {
            IWebHost host = BuildHost();
            host.Run(cancellationTkn);
        }

        private static IWebHost BuildHost()
        {
            return new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
        }
    }
}
