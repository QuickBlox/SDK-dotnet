using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Nito.AsyncEx;
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

        private const string imagesFolder = "images";
        private const string fileNameFormat = "{0}.jpg";
        private readonly IQuickbloxClient quickbloxClient;
        private readonly IFileStorage fileStorage;
        private static readonly List<int> thisSessionImages = new List<int>(); // image links that were loaded during this session of application
        private readonly AsyncLock listLock = new AsyncLock();

        #endregion

        #region Ctor

        public ImagesService(IQuickbloxClient quickbloxClient, IFileStorage fileStorage)
        {
            this.fileStorage = fileStorage;
            this.quickbloxClient = quickbloxClient;
        }

        #endregion

        #region IImageService Members

        public async Task<ImageSource> GetPrivateImage(int imageUploadId)
        {
            if (thisSessionImages.Contains(imageUploadId))
                return await GetImageFromStorage(imageUploadId);

            return await GetImageFromServer(imageUploadId);
        }

        #endregion

        #region Private methods

        private async Task<ImageSource> GetImageFromServer(int imageUploadId)
        {
            var downloadResponse = await quickbloxClient.ContentClient.DownloadFileById(imageUploadId);
            if (downloadResponse.StatusCode == HttpStatusCode.OK)
            {
                await fileStorage.WriteToFile(imagesFolder, string.Format(fileNameFormat, imageUploadId), downloadResponse.Result);
                using (await listLock.LockAsync())
                {
                    thisSessionImages.Add(imageUploadId);
                }
                return await CreateBitmapImage(downloadResponse.Result);
            }

            return null;
        }

        private async Task<ImageSource> GetImageFromStorage(int imageUploadId)
        {
            var imageBytes = await fileStorage.ReadFile(imagesFolder, string.Format(fileNameFormat, imageUploadId));
            if (imageBytes != null)
            {
                return await CreateBitmapImage(imageBytes);
            }

            return null;
        }

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
