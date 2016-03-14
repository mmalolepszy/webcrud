using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting;

namespace WebCRUD.vNext
{
    public class Program
    {
        // Entry point for the application.
        public static void Main(string[] args)
        {
            if(args.Length > 0 && args.Any(x => x == "debug"))
            {
                args = args.Except(new string[] { "debug" }).ToArray();

                Console.WriteLine($"Process Id: {Process.GetCurrentProcess().Id}");
                Console.WriteLine("Waiting for Debugger to attach...");
                SpinWait.SpinUntil(() => Debugger.IsAttached);
            }

            var host = new WebHostBuilder()
                .UseDefaultConfiguration(args)
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
