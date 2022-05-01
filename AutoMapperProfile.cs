using AutoMapper;
using VideoStore_Backend.Dtos.FavoriteList;
using VideoStore_Backend.Dtos.Gallery;
using VideoStore_Backend.Dtos.User;
using VideoStore_Backend.Dtos.Video;
using VideoStore_Backend.Models;

namespace VideoStore_Backend
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<Gallery, GetGalleryDto>();   
            CreateMap<Gallery, GetGalleryWithVideosDto>();
            CreateMap<Video, GetVideoDto>();
            CreateMap<CreateFavoriteListDto, FavoriteList>();
            CreateMap<FavoriteList, GetFavoriteListDto>();
        }
    }
}