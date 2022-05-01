using System;
using System.Collections.Generic;
using VideoStore_Backend.Dtos.Video;

namespace VideoStore_Backend.Dtos.Gallery
{
    public class GetGalleryWithVideosDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public List<GetVideoDto> Videos { get; set; }

    }
}