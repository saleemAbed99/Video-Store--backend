using System.Collections.Generic;

namespace VideoStore_Backend.Models
{
    public class FavoriteList
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string UserUsername { get; set; }
        public List<Video> Videos { get; set; }
    }
}