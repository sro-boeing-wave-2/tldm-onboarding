using Onboarding.Contract;
using Chilkat;
using Consul;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Onboarding.Services
{
    public class JWTTokenService : IJWTTokenService
    {
        public JWTTokenService()
        {
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
            using (var client = new ConsulClient())
            {
                // client.Config.Address = new Uri("http://10.0.75.1:8500");
                //for aws
                client.Config.Address = new Uri("http://"+Environment.GetEnvironmentVariable("MACHINE_LOCAL_IPV4")+":8500");
                var putPair = new KVPair("secretkey")
                {

                    Value = Encoding.UTF8.GetBytes(publicKey)
                };
                var putAttempt = await client.KV.Put(putPair);
                if (putAttempt.Response)
                {
                    return true;                    
                }
            }
            return false;
            // Push the key to consul here
        }
        public string GetToken(JsonObject payload)
        {
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