namespace Quickblox.Sdk.Cryptographic
{
    public interface ICryptographicProvider
    {
        string Encrypt(string key, string authSecret);
    }
}
