using Chilkat;

namespace Onboarding.Contract
{
    public interface IJWTTokenService
    {
        string GetToken(JsonObject payload);
        //PrivateKey PrivateKey { get; set; }
    }
}