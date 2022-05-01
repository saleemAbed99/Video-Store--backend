using System;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Dtos.Gallery
{
    public class GetGalleryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public int NumberOfVideos { get; set; }
    }
}