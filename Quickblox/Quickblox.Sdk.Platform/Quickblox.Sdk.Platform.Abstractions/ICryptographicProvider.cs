namespace Quickblox.Sdk.Platform
{
    public interface ICryptographicProvider
    {
        string Encrypt(string key, string authSecret);
    }
}
