using System;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace Quickblox.Sdk.Platform
{
    public class HmacSha1CryptographicProvider : ICryptographicProvider
    {
        public string Encrypt(string baseString, string keyString)
        {
            var crypt = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
            var buffer = CryptographicBuffer.ConvertStringToBinary(baseString, BinaryStringEncoding.Utf8);
            var keyBuffer = CryptographicBuffer.ConvertStringToBinary(keyString, BinaryStringEncoding.Utf8);
            var key = crypt.CreateKey(keyBuffer);

            var sigBuffer = CryptographicEngine.Sign(key, buffer);
            //string signature = CryptographicBuffer.EncodeToBase64String(sigBuffer);
            string signature = CryptographicBuffer.EncodeToHexString(sigBuffer);
            return signature;
        }

        void CreateHMAC(String strMsg, String strAlgName, out IBuffer buffMsg, out CryptographicKey hmacKey, out IBuffer buffHMAC)
        {
            // Create a MacAlgorithmProvider object for the specified algorithm.
            MacAlgorithmProvider objMacProv = MacAlgorithmProvider.OpenAlgorithm(strAlgName);

            // Demonstrate how to retrieve the name of the algorithm used.
            String strNameUsed = objMacProv.AlgorithmName;

            // Create a buffer that contains the message to be signed.
            BinaryStringEncoding encoding = BinaryStringEncoding.Utf8;
            buffMsg = CryptographicBuffer.ConvertStringToBinary(strMsg, encoding);

            // Create a key to be signed with the message.
            IBuffer buffKeyMaterial = CryptographicBuffer.GenerateRandom(objMacProv.MacLength);
            hmacKey = objMacProv.CreateKey(buffKeyMaterial);

            // Sign the key and message together.
            buffHMAC = CryptographicEngine.Sign(hmacKey, buffMsg);

            // Verify that the HMAC length is correct for the selected algorithm
            if (buffHMAC.Length != objMacProv.MacLength)
            {
                throw new Exception("Error computing digest");
            }
        }
    }
}
