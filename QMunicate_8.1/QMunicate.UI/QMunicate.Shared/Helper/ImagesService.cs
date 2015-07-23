using System;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Quickblox.Sdk;

namespace QMunicate.Helper
{
    public interface IImageService
    {
        Task<ImageSource> GetPrivateImage(int imageUploadId);
    }

    public class ImagesService : IImageService
    {
        #region Fields

        private readonly IQuickbloxClient quickbloxClient;

        #endregion

        #region Ctor

        public ImagesService(IQuickbloxClient quickbloxClient)
        {
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region IImageService Members

        public async Task<ImageSource> GetPrivateImage(int imageUploadId)
        {
            var downloadResponse = await quickbloxClient.ContentClient.DownloadFileById(imageUploadId);
            if (downloadResponse.StatusCode == HttpStatusCode.OK)
            {
                return await CreateBitmapImage(downloadResponse.Result);
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Private methods

        private async Task<BitmapImage> CreateBitmapImage(byte[] imageBytes)
        {
            if (imageBytes == null) return null;

            try
            {
                var bitmapImage = new BitmapImage();

                var stream = new InMemoryRandomAccessStream();
                await stream.WriteAsync(imageBytes.AsBuffer());
                stream.Seek(0);

                bitmapImage.SetSource(stream);
                return bitmapImage;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

    }
}
