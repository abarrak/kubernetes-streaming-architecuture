using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using StreamingCore.Models.Contracts;

namespace StreamingCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StreamingController : ControllerBase
    {
        private readonly IMediaStreamingManager _manager;

        public StreamingController(IMediaStreamingManager manager)
        {
            _manager = manager;
        }

        [HttpGet("range-stream")]
        public IActionResult StreamInRanges(Guid fileId)
        {
            var url = _manager.GetMediaFilePath(fileId);

            return PhysicalFile(url, "application/octet-stream", enableRangeProcessing: true);
        }
    }
}
