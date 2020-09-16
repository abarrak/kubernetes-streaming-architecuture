using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using StreamingCore.Models.Contracts;

namespace StreamingCore.Models.BussinessLogic
{
    public class MediaStreamingManager : IMediaStreamingManager
    {
        private readonly IConfiguration _configuration;

        public MediaStreamingManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetMediaFilePath(Guid id)
        {
            var storagePath = _configuration.GetValue<string>("FileStorePath");

            var fullPath = Path.Combine(storagePath, id.ToString());

            return fullPath;
        }
    }
}
