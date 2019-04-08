using NorthwindSystem.Models;
using System;
using System.IO;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;

namespace NorthwindSystem.Helpers
{
    public class FileImageCacheHelper : IImageCacheHelper
    {
        private const string imageExtension = "bmp";
        private CachingOptions _options;
        private Timer _timer;

        public void InitCacheOptions(CachingOptions cacheOptions)
        {
            _options = cacheOptions;
            _timer = new Timer()
            {
                Interval = _options.CacheExpirationTime.TotalMilliseconds,
                AutoReset = true
            };
            
            _timer.Elapsed += (s, e) => OnTimedElapsed();

            if (!Directory.Exists(_options.DirectoryPath))
            {
                Directory.CreateDirectory(_options.DirectoryPath);
            }
        }

        public async Task<Stream> Get(string key)
        {
            _timer.Stop();

            var stream = await GetCachedImage(key);

            _timer.Start();
            return stream;
        }

        public async Task Save(Stream imageStream, string key)
        {
            string imageFilePath = GetImageFileFullPath(key);
            var filesNumber = Directory.EnumerateFiles(_options.DirectoryPath).Count();

            if (filesNumber < _options.MaxImagesCount)
            {
                using (var fileStream = new FileStream(imageFilePath, FileMode.Create, FileAccess.Write))
                {
                    var memoryStream = imageStream as MemoryStream;
                    await fileStream.WriteAsync(memoryStream.ToArray());
                }
            }
        }

        private async Task<Stream> GetCachedImage(string key)
        {
            string imageFilePath = GetImageFileFullPath(key);
            if (!File.Exists(imageFilePath))
            {
                return null;
            }

            MemoryStream stream = new MemoryStream();
            using (var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                await fileStream.CopyToAsync(stream);
            }
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        private string GetImageFileFullPath(string key)
        {
            return $"{_options.DirectoryPath}/{key}.{imageExtension}";
        }

        private void OnTimedElapsed()
        {
            Clean();
        }

        private void Clean()
        {
            if (Directory.Exists(_options.DirectoryPath))
            {
                var directory = new DirectoryInfo(_options.DirectoryPath);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
        }
    }
}
