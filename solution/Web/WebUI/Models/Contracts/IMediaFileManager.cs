using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Models.Dtos;
using WebUI.Models.Entities;

namespace WebUI.Models.Contracts
{
    public interface IMediaFileManager
    {
        Task<bool> Upload(FileDto file);

        Task<MediaFile> Get(long id);
        Task<IEnumerable<MediaFile>> GetAll();
    }
}
