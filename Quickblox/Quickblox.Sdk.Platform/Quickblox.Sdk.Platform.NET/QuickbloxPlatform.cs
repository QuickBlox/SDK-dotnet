namespace Quickblox.Sdk.Platform
{
    public static class QuickbloxPlatform
    {
        public static void Init()
        {
            Resolver.Register<ICryptographicProvider, HmacSha1CryptographicProvider>();
        }
    }
}