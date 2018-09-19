using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Onboarding
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Chilkat.Global glob = new Chilkat.Global();
            // glob.UnlockBundle("Anything for 30-day trial");

            // Chilkat.Rsa rsaKey = new Chilkat.Rsa();

            // rsaKey.GenerateKey(1024);
            // var rsaPrivKey = rsaKey.ExportPrivateKeyObj();

            // var rsaPublicKey = rsaKey.ExportPublicKeyObj();
            // var rsaPublicKeyAsString = rsaKey.ExportPublicKey();

            // using (var client = new ConsulClient())
            // {
            //     var putPair = new KVPair("secretkey")
            //     {
            //         Value = Encoding.UTF8.GetBytes(rsaPublicKeyAsString)
            //     };
            // }

                CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
