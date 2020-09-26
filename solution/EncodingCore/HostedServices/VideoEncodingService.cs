using System;
using System.Threading;
using System.Threading.Tasks;
using EncodingCore.Models.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EncodingCore.HostedServices
{
    public class VideoEncodingService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private IVideoEncodingManager _manager;

        public VideoEncodingService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessEncodingWorkload();
                await Task.Delay(TimeSpan.FromSeconds(60));
            }
        }

        private async Task ProcessEncodingWorkload()
        {
            using (var scope = _services.CreateScope())
            {
                _manager = scope.ServiceProvider.GetRequiredService<IVideoEncodingManager>();

                await _manager.Process();
            }
        }
    }
}
