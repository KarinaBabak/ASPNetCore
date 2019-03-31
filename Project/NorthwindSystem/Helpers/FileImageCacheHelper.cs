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
                Interval = GetCacheExpirationPeriod(),
                AutoReset = false
            };
            _timer.Elapsed += (s, e) => OnTimedElapsed();

            if (!Directory.Exists(_options.DirectoryPath))
            {
                Directory.CreateDirectory(_options.DirectoryPath);
            }
        }

        public async Task<Stream> GetOrCreate(string key)
        {
            // TODO: reset timer
            string imageFilePath = GetImageFileFullPath(key);
            using (MemoryStream stream = new MemoryStream())
            {
                if (!File.Exists(imageFilePath))
                {
                    await Create(stream, imageFilePath);
                }
                else
                {
                    using (var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
                    {
                        await fileStream.CopyToAsync(stream);
                    }
                }
                return stream;
            }            
        }

        private async Task Create(MemoryStream imageStream, string imageFilePath)
        {
            var filesNumber = Directory.EnumerateFiles(_options.DirectoryPath).Count();
            if (filesNumber < _options.MaxImagesCount)
            {
                using (var fileStream = new FileStream(imageFilePath, FileMode.Create, FileAccess.Write))
                {
                    //fileStream.Seek(0, SeekOrigin.End);
                    //await fileStream.WriteAsync(imageStream.ToArray());
                    //imageStream.Seek(0, SeekOrigin.Begin);
                    imageStream.CopyTo(fileStream);
                }
            }
        }

        private string GetImageFileFullPath(string key)
        {
            return $"{_options.DirectoryPath}/{key}.{imageExtension}";
        }

        private void OnTimedElapsed()
        {
            Clean();
            RefreshCacheExpirationTime();
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

        private void RefreshCacheExpirationTime()
        {
            _timer.Interval = GetCacheExpirationPeriod();
        }

        private double GetCacheExpirationPeriod()
        {
            return (DateTime.Now + _options.CacheExpirationTime).Millisecond;
        }
    }
}
