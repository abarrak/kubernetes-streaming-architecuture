using System;

namespace StreamingCore.Models.Contracts
{
    public interface IMediaStreamingManager
    {
        string GetMediaFilePath(Guid id);
    }
}
