using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace VideoStore_Backend.Dtos.Video
{
    public class UploadVideoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Files { get; set; }
        public int GalleryId { get; set; }
    }
}