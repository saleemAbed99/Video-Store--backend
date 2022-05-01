using System.Collections.Generic;

namespace VideoStore_Backend.Models
{
    public class Video
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
        public string Description { get; set; }
        public Gallery Gallery { get; set; }
        public List<FavoriteList> FavoriteList { get; set; }
    }
}