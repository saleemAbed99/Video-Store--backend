using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using VideoStore_Backend.Data;
using VideoStore_Backend.Dtos.Gallery;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Services.Gallery
{
    public class GalleryService : IGalleryService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public GalleryService(DataContext context, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _context = context;
            _mapper = mapper;
            _httpContext = httpContext;
        }
        private int GetUserID() => int.Parse(_httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        private string GetUserRole() => _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        
        public async Task<ServiceResponse<GetGalleryDto>> CreateGallery(CreateGalleryDto gallery)
        {
            ServiceResponse<GetGalleryDto> response = new ServiceResponse<GetGalleryDto>();

            try
            {
                if (GetUserRole().ToLower().Equals("admin")) 
                {
                    if (await _context.Galleries.AnyAsync(x => x.Name == gallery.Name))
                    {
                        response.Success = false;
                        response.Message = "Gallery name already exists.";
                    }
                    else
                    {
                        User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == GetUserID());

                        Models.Gallery newGallery = new Models.Gallery
                        {
                            Name = gallery.Name,
                            Description = gallery.Description,
                            DateCreated = DateTime.Now,
                            User = user
                        };

                        _context.Galleries.Add(newGallery);
                        await _context.SaveChangesAsync();
                        response.Data = _mapper.Map<GetGalleryDto>(newGallery);
                        response.Data.CreatedBy = user.Username;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "You are not authorized to create a gallery.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteGallery(int id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();

            try
            {
                var gallery = await _context.Galleries.Include(g => g.Videos).Include(g => g.User).FirstOrDefaultAsync(x => x.Id == id);
                if (GetUserRole().ToLower().Equals("admin") || gallery.User.Id == GetUserID())
                {
                    if (gallery == null)
                    {
                        response.Success = false;
                        response.Message = "Gallery not found.";
                    }
                    if(gallery.Videos.Count > 0)
                    {
                        response.Success = false;
                        response.Message = "Gallery has videos. Please delete the videos first.";
                    }
                    else
                    {
                        _context.Galleries.Remove(gallery);
                        await _context.SaveChangesAsync();
                        response.Data = true;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "You are not authorized to update this gallery.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetGalleryWithVideosDto>> GetGallery(int id)
        {
            ServiceResponse<GetGalleryWithVideosDto> response = new ServiceResponse<GetGalleryWithVideosDto>();

            try
            {
                var gallery = await _context.Galleries.Include(g => g.User).Include(g => g.Videos).FirstOrDefaultAsync(x => x.Id == id);
                if (gallery == null)
                {
                    response.Success = false;
                    response.Message = "Gallery not found.";
                }
                else
                {   
                    response.Data = _mapper.Map<GetGalleryWithVideosDto>(gallery);
                    response.Data.CreatedBy = gallery.User.Username;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetGalleryDto>>> ListGalleries()
        {
              ServiceResponse<List<GetGalleryDto>> response = new ServiceResponse<List<GetGalleryDto>>();

            try
            {
                var galleries = await _context.Galleries.Include(g => g.User).Include(g => g.Videos).ToListAsync();
                if (galleries.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No Galleries.";
                }
                else
                {   
                    response.Data = galleries.Select(x => {
                         var obj = _mapper.Map<GetGalleryDto>(x);
                         obj.CreatedBy = x.User.Username;
                         obj.NumberOfVideos = x.Videos.Count;
                         return obj;
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetGalleryDto>> UpdateGallery(JsonPatchDocument<Models.Gallery> galleryUpdates, int id)
        {
            ServiceResponse<GetGalleryDto> response = new ServiceResponse<GetGalleryDto>();

            try
            {
                var gallery = await _context.Galleries.FirstOrDefaultAsync(x => x.Id == id);
                if (GetUserRole().ToLower().Equals("admin") || gallery.User.Id == GetUserID())
                {
                    if (gallery == null)
                    {
                        response.Success = false;
                        response.Message = "Gallery not found.";
                    }
                    else
                    {
                        galleryUpdates.ApplyTo(gallery);
                        await _context.SaveChangesAsync();
                        response.Data = _mapper.Map<GetGalleryDto>(gallery);
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "You are not authorized to update this gallery.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<ServiceResponse<List<GetGalleryDto>>> ListAdminGalleries()
        {
            ServiceResponse<List<GetGalleryDto>> response = new ServiceResponse<List<GetGalleryDto>>();

            try
            {
                var galleries = await _context.Galleries.Where(g => g.User.Id == GetUserID()).Include(g => g.User).Include(g => g.Videos).ToListAsync();

                if (GetUserRole().ToLower().Equals("admin"))
                {
                    if (galleries.Count == 0)
                    {
                        response.Success = false;
                        response.Message = "No Galleries.";
                    }
                    else
                    {   
                        response.Data = galleries.Select(x => {
                         var obj = _mapper.Map<GetGalleryDto>(x);
                         obj.CreatedBy = x.User.Username;
                         obj.NumberOfVideos = x.Videos.Count;
                         return obj;
                        }).ToList();
                    }  
                }
                else
                {
                    response.Success = false;
                    response.Message = "You are not authorized to list galleries.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}