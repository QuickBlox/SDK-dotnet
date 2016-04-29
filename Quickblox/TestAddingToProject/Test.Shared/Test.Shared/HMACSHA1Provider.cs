using System;
using System.Security.Cryptography;
using System.Text;
using Quickblox.Sdk.Cryptographic;

namespace Test.Shared
{
    public class HMACSHA1Provider : ICryptographicProvider
    {
        public string Encrypt(string key, string signatureBase)
        {
            var keyBytes = Encoding.UTF8.GetBytes(signatureBase);
            using (HMACSHA1 hashAlgorithm = new HMACSHA1(keyBytes))
            {
                byte[] dataBuffer = Encoding.UTF8.GetBytes(key);
                byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
