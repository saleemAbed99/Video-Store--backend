using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using VideoStore_Backend.Data;
using VideoStore_Backend.Dtos.FavoriteList;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Services.FavoriteList
{
    public class FavoriteListService : IFavoriteListService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public FavoriteListService(DataContext context, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;
        }

        private int GetUserID() => int.Parse(_httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        private string GetUserRole() => _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        public async Task<bool> CreateFavoriteList(CreateFavoriteListDto favoriteList)
        {
            bool response = false;

            try
            {        
                User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == favoriteList.User.Id);

                var newFavoriteList = _mapper.Map<Models.FavoriteList>(favoriteList);
                
                _context.FavoriteLists.Add(newFavoriteList);
                await _context.SaveChangesAsync();
                
                response = true;
              
            }
            catch (System.Exception ex)
            {
                response = false;
            }

            return response;
        }

        public async Task<ServiceResponse<GetFavoriteListDto>> GetFavoriteList()
        {
            ServiceResponse<GetFavoriteListDto> response = new ServiceResponse<GetFavoriteListDto>();

            try
            {
                var user = await _context.Users.Include(u => u.FavoriteList).FirstOrDefaultAsync(x => x.Id == GetUserID()); 
                Models.FavoriteList favoriteList = await _context.FavoriteLists.Include(f => f.User).Include(f => f.Videos).ThenInclude(v => v.Gallery).FirstOrDefaultAsync(x => x.Id == user.FavoriteList.Id);

                if (favoriteList == null)
                {
                    response.Success = false;
                    response.Message = "Favorite List not found.";
                }
                if(favoriteList.User.Id != GetUserID())
                {
                    response.Success = false;
                    response.Message = "You are not authorized to view this favorite list.";
                }
                else
                {
                    response.Data = _mapper.Map<GetFavoriteListDto>(favoriteList);
                    response.Success = true;
                }
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetFavoriteListDto>> AddVideoToFavoriteList(HandleFavoriteListDto input)
        {
            ServiceResponse<GetFavoriteListDto> response = new ServiceResponse<GetFavoriteListDto>();

            try
            {
                var user = await _context.Users.Include(u => u.FavoriteList).FirstOrDefaultAsync(x => x.Id == GetUserID()); 
                Models.FavoriteList favoriteList = await _context.FavoriteLists.Include(f => f.Videos).Include(f => f.User).FirstOrDefaultAsync(x => x.Id == user.FavoriteList.Id);


                if (favoriteList == null)
                {
                    response.Success = false;
                    response.Message = "Favorite List not found.";
                }
                if (favoriteList.User.Id != GetUserID())
                {
                    response.Success = false;
                    response.Message = "You are not authorized to add videos to this list.";
                }
                else
                {
                    Models.Video video = await _context.Videos.FirstOrDefaultAsync(x => x.Id == input.VideoId);

                    if (video == null)
                    {
                        response.Success = false;
                        response.Message = "Video not found.";
                    }
                    else
                    {
                        favoriteList.Videos.Add(video);
                        await _context.SaveChangesAsync();

                        response.Data = _mapper.Map<GetFavoriteListDto>(favoriteList);
                        response.Success = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteVideoFromFavoriteList(int id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();

            try
            {
                var user = await _context.Users.Include(u => u.FavoriteList).FirstOrDefaultAsync(x => x.Id == GetUserID()); 
                Models.FavoriteList favoriteList = await _context.FavoriteLists.Include(f => f.Videos).Include(f => f.User).FirstOrDefaultAsync(x => x.Id == user.FavoriteList.Id);

                if (favoriteList == null)
                {
                    response.Success = false;
                    response.Message = "Favorite List not found.";
                }
                if (favoriteList.User.Id != GetUserID())
                {
                    response.Success = false;
                    response.Message = "You are not authorized to delete videos from this list.";
                }
                else
                {
                    Models.Video video = await _context.Videos.FirstOrDefaultAsync(x => x.Id == id);

                    if (video == null)
                    {
                        response.Success = false;
                        response.Message = "Video not found.";
                    }
                    else
                    {
                        favoriteList.Videos.Remove(video);
                        await _context.SaveChangesAsync();

                        response.Data = true;
                        response.Success = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

       
    }
}