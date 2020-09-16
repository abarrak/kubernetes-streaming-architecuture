using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;
using WebUI.Models.Contracts;
using WebUI.Models.Dtos;
using WebUI.Models.Entities;

namespace WebUI.Models.BussinessLogic
{
    public class MediaFileManager : IMediaFileManager
    {
        private readonly IMediaFileRepository _repository;
        private readonly IConfiguration _configuration;

        private long _maxSizeLimit;
        private string _storagePath;
        private List<string> _allowedExtensions;

        public MediaFileManager(
            IMediaFileRepository repository,
            IConfiguration configuration
        )
        {
            _repository = repository;
            _configuration = configuration;

            LoadFileSettings();
        }

        private void LoadFileSettings()
        {
            _maxSizeLimit = long.Parse(_configuration.GetValue<string>("FileMaxSizeLimit"));
            _storagePath = _configuration.GetValue<string>("FileStorePath");
            _allowedExtensions = _configuration.GetValue<string>("AllowedExtensions").Split(",").ToList();
        }

        public async Task<MediaFile> Get(long id)
        {
            var mediaFile = await _repository.Find(id);
            MapFullPaths(mediaFile);

            return mediaFile;
        }

        public async Task<IEnumerable<MediaFile>> GetAll()
        {
            return await _repository.FindAll();
        }

        public async Task<bool> Upload(FileDto file)
        {
            if (file == null || file.FormFile?.Length <= 0)
            {
                return false;
            }

            if (file.FormFile?.Length > _maxSizeLimit)
            {
                return false;
            }

            var extension = Path.GetExtension(file.FormFile.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
            {
                return false;
            }


            // Store to the file system.
            //
            var filePath = Guid.NewGuid().ToString();
            var fileFullPath = Path.Combine(_storagePath, filePath, Path.GetRandomFileName());

            fileFullPath = Path.ChangeExtension(fileFullPath, extension);
            Directory.CreateDirectory(filePath);

            using (var stream = File.Create(fileFullPath))
            {
                await file.FormFile.CopyToAsync(stream);
            }

            // Build the media file entity and persist it.
            //

            var mediaFile = new MediaFile
            {
                Name = HttpUtility.HtmlEncode(Path.GetFileNameWithoutExtension(file.FormFile.FileName)),
                FilePath = fileFullPath,
                MainfestPath = filePath,
                Size = file.FormFile.Length,
                Type = extension,
                Description = HttpUtility.HtmlEncode(file.Desciption),
                UploadedAt = DateTime.UtcNow
            };

            await _repository.Add(mediaFile);
            return true;
        }

        private void MapFullPaths(MediaFile mediaFile)
        {
            var streamingUrl = _configuration.GetValue<string>("StreamingServerUrl");
            mediaFile.MainfestPath = $"{streamingUrl}/{mediaFile.MainfestPath}";
        }
    }
}
