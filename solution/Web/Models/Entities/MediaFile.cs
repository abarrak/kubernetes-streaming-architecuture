using System;

namespace WebUI.Models.Entities
{
    public class MediaFile
    {
        public long Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string Description { get; set; }

        public string FilePath { get; set; }
        public string MainfestPath { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
