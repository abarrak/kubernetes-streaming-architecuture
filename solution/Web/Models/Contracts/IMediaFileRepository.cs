using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebUI.Models.Entities;

namespace WebUI.Models.Contracts
{
    public interface IMediaFileRepository
    {
        Task<IEnumerable<MediaFile>> FindAll();
        Task<MediaFile> Find(long id);
        Task Add(MediaFile file);
    }
}
