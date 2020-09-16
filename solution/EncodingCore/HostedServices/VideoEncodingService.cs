using System;
using System.Threading;
using System.Threading.Tasks;
using EncodingCore.Models.Contracts;
using Microsoft.Extensions.Hosting;

namespace EncodingCore.HostedServices
{
    public class VideoEncodingService : BackgroundService
    {
        private readonly IVideoEncodingManager _ecodingManager;

        public VideoEncodingService(IVideoEncodingManager encodingManager)
        {
            _ecodingManager = encodingManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
