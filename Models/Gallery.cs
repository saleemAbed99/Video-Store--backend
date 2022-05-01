using System;
using System.Collections.Generic;

namespace VideoStore_Backend.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public User User { get; set; }
        public List<Video> Videos { get; set; }
    }
}