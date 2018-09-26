using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using DotNetEnv;
namespace Onboarding
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Env.Load("./machine_config/.env");
                Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4");
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't find folder machine_config");
            }
            CreateWebHostBuilder(args).Build().Run();
            
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
