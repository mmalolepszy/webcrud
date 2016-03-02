using System;
using Microsoft.AspNetCore.Hosting;

namespace WebCRUD.vNext
{
    public class Program
    {
        // Entry point for the application.
        public static void Main(string[] args)
        {
            IWebHostBuilder hostBuilder = new WebHostBuilder();
            hostBuilder = hostBuilder.UseDefaultConfiguration(args);
            hostBuilder = hostBuilder.UseIISPlatformHandlerUrl();
            hostBuilder = hostBuilder.UseStartup<Startup>();
            IWebHost host = hostBuilder.Build();

            host.Run();
        }
    }
}
