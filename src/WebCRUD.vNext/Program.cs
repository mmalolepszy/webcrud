using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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

            var config = new ConfigurationBuilder()  
                .AddJsonFile("hosting.json", optional: true)
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .AddCommandLine(args)
                .Build(); 

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .CaptureStartupErrors(true)
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();

            host.Run();
        }
    }
}
