using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using WebUI.Models.Contracts;
using WebUI.Models.Dtos;
using WebUI.Models.Entities;

namespace WebUI.Models.BussinessLogic
{
    public class MediaFileManager : IMediaFileManager
    {
        private readonly IMediaFileRepository _repository;

        public MediaFileManager(IMediaFileRepository repository)
        {
            _repository = repository;
        }

        public async Task<MediaFile> Get(long id)
        {
            return await _repository.Find(id);
        }

        public async Task<IEnumerable<MediaFile>> GetAll()
        {
            return await _repository.FindAll();
        }

        public async Task Upload(FileDto file)
        {
            // Store to the file system.

            // Build the media file entity and persist it.
            var mediaFile = new MediaFile
            {
                Name = "",
                FilePath = Path.GetRandomFileName(),                
                Description = HttpUtility.HtmlEncode(file.Desciption)
            };

            await _repository.Add(mediaFile);
        }
    }
}
