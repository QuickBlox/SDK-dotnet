using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Profile;
using QMunicate.Core.MessageService;

namespace QMunicate.Helper
{
    public class Helpers
    {
        public static string GetHardwareId()
        {
            var token = HardwareIdentification.GetPackageSpecificToken(null);
            return CryptographicBuffer.EncodeToBase64String(token.Id);
        }

        public static string ComputeMD5(string str)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(str, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }

        public async static Task ShowErrors(Dictionary<string, string[]> errorsDictionary, IMessageService messageService)
        {
            if (messageService == null || errorsDictionary == null || errorsDictionary.Count == 0) return;

            foreach (var error in errorsDictionary)
            {
                foreach (string errorMessage in error.Value)
                {
                    await messageService.ShowAsync(error.Key, errorMessage);
                }
            }
        }
    }
}
