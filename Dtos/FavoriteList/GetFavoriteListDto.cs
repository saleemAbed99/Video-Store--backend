using System.Collections.Generic;
using VideoStore_Backend.Dtos.Video;

namespace VideoStore_Backend.Dtos.FavoriteList
{
    public class GetFavoriteListDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserUsername { get; set; }
        public List<GetVideoDto> Videos { get; set; }
    }
}