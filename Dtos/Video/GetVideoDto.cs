using System.Collections.Generic;
using VideoStore_Backend.Dtos.FavoriteList;

namespace VideoStore_Backend.Dtos.Video
{
    public class GetVideoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
        public string Description { get; set; }
        public int GalleryId { get; set; }
        public string GalleryName { get; set; }
    }
}