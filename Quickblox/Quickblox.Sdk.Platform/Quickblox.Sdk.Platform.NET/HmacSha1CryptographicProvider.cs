using System;
using System.Security.Cryptography;
using System.Text;

namespace Quickblox.Sdk.Platform
{
    public class HmacSha1CryptographicProvider : ICryptographicProvider
    {
        public string Encrypt(string key, string signatureBase)
        {
            var keyBytes = Encoding.UTF8.GetBytes(signatureBase);
            HMACSHA1 hashAlgorithm = new HMACSHA1(keyBytes);
            byte[] dataBuffer = Encoding.UTF8.GetBytes(key);
            byte[] hashBytes = hashAlgorithm.ComputeHash(dataBuffer);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); ;
        }
    }
}
