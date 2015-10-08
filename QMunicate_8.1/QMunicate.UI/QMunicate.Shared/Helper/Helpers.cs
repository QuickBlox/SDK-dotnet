using QMunicate.Core.Logger;
using QMunicate.Core.MessageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

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

        public static string GetAppVersion()
        {

            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;

            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        public static bool IsInternetConnected()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        public static int GetUserIdFromJid(string jid)
        {
            int senderId;
            var jidParts = jid.Split('/');
            if (int.TryParse(jidParts.Last(), out senderId))
                return senderId;

            return 0;
        }

        public static async Task<ImageSource> CreateBitmapImage(byte[] imageBytes, int? decodePixelWidth = null, int? decodePixelHeight = null)
        {
            if (imageBytes == null) return null;

            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(imageBytes.AsBuffer());
                stream.Seek(0);

                return CreateBitmapImage(stream, decodePixelWidth, decodePixelHeight);
            }
        }

        public static ImageSource CreateBitmapImage(IRandomAccessStream imageBytesStream, int? decodePixelWidth = null, int? decodePixelHeight = null)
        {
            if (imageBytesStream == null) return null;

            try
            {
                var bitmapImage = new BitmapImage();
                if (decodePixelWidth.HasValue) bitmapImage.DecodePixelWidth = decodePixelWidth.Value;
                if (decodePixelHeight.HasValue) bitmapImage.DecodePixelHeight = decodePixelHeight.Value;
                bitmapImage.SetSource(imageBytesStream);
                return bitmapImage;
            }
            catch (Exception ex)
            {
                QmunicateLoggerHolder.Log(QmunicateLogLevel.Debug, "Helpers class. Failed to create BitmapImage. " + ex);
                return null;
            }
        }
    }
}
