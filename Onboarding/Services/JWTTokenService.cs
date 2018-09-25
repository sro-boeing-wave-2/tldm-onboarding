using Onboarding.Contract;
using Chilkat;
using Consul;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

namespace Onboarding.Services
{
    public class JWTTokenService : IJWTTokenService
    {
        // public IConfiguration Configuration { get; }
        //[System.Runtime.InteropServices.ComVisible(true)]
        //public static class Environment { };
        public JWTTokenService()
        {
            //Console.WriteLine("YOYO "+ Environment.)
           //s Configuration = configuration;
            Chilkat.Global global = new Chilkat.Global();
            global.UnlockBundle("Anything for 30-day trail");
            Chilkat.Rsa rsaKey = new Chilkat.Rsa();
            rsaKey.GenerateKey(1024);
            PrivateKey = rsaKey.ExportPrivateKeyObj();

            AddPublicKeyToConsul(rsaKey.ExportPublicKey()).Wait();
 
            Console.WriteLine("\n" + "hello boss" + "\n");
        }

        public static async Task<bool> AddPublicKeyToConsul(string publicKey)
        {
            Console.WriteLine("\n" + "hello boss1" + "\n");

            //var clientConfig = new ConsulClientConfiguration
            //{
            //    Address = new Uri("http://10.0.75.1:8500")
            //};
            using (var client = new ConsulClient())
            {
                Console.WriteLine("\n" + "hello boss2" + "\n");
                client.Config.Address = new Uri("http://10.0.75.1:8500");
                //for aws
               // client.Config.Address = new Uri("http://localhost:8500");
                var putPair = new KVPair("secretkey")
                {

                    Value = Encoding.UTF8.GetBytes(publicKey)
                };
                Console.WriteLine("\n" + "hello boss3" + "\n");
                var putAttempt = await client.KV.Put(putPair);
                Console.WriteLine("\n" +"hi" + putAttempt.Response + "\n");
                if (putAttempt.Response)
                {
                    Console.WriteLine("\n" + "hello boss4 " + "\n");
                    return true;
                    
                }
            }
            return false;
            // Push the key to consul here
        }
        public string GetToken(JsonObject payload)
        {
            var variable = "hello";
            Console.WriteLine(Environment.GetEnvironmentVariable(variable));


            // Use the private key to generate token
            JsonObject jwtHeader = new JsonObject();
            jwtHeader.AppendString("alg", "RS256");
            jwtHeader.AppendString("typ", "JWT");
            Jwt jwt = new Jwt();
            string token = jwt.CreateJwtPk(jwtHeader.Emit(), payload.Emit(), PrivateKey);
            return token;
        }

        private PrivateKey PrivateKey { get; set; }

    }
}