using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoStore_Backend.Data;
using VideoStore_Backend.Dtos.Video;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Services.Video
{
    public class VideoService : IVideoService
    {
        private readonly IWebHostEnvironment _env;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;



        public VideoService(DataContext context, IMapper mapper, IWebHostEnvironment env, IHttpContextAccessor httpContext)
        {
            _context = context;
            _env = env;
            _httpContext = httpContext;
            _mapper = mapper;
        }

        private string GetUserRole() => _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        private int GetUserID() => int.Parse(_httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
    
        public async Task<ServiceResponse<GetVideoDto>> UploadVideo(UploadVideoDto video)
        {
            var response = new ServiceResponse<GetVideoDto>();

            try
            {
                if (GetUserRole().ToLower().Equals("admin")) {

                    if(video.Files.Count == 0)
                    {
                        response.Success = false;
                        response.Message = "No Video Uploaded.";
                        return response;
                    }

                    string folderName = Path.Combine("Resources", "Videos");
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fileName = ContentDispositionHeaderValue.Parse(video.Files[0].ContentDisposition).FileName.Trim('"');
                    var dbPath = Path.Combine(folderName, fileName);
                    
                    using (var stream = new FileStream(Path.Combine(pathToSave, fileName), FileMode.Create))
                    {
                        video.Files[0].CopyTo(stream);
                    }

                    var gallery = await _context.Galleries.FirstOrDefaultAsync(g => g.Id == video.GalleryId);

                    var newVideo = new Models.Video
                    {
                        Title = video.Title,
                        Description = video.Description,
                        Uri = dbPath,
                        Gallery = gallery
                    };

                    _context.Videos.Add(newVideo);

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetVideoDto>(newVideo);

                }

             }
             catch (System.Exception ex)
             {
                response.Success = false;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }
            
            return response;
        }

        public async Task<ServiceResponse<GetVideoDto>> GetVideo(int id)
        {
            var response = new ServiceResponse<GetVideoDto>();

            try
            {
                var video = await _context.Videos.Include(v => v.Gallery).FirstOrDefaultAsync(v => v.Id == id);

                if (video == null)
                {
                    response.Success = false;
                    response.Message = "Video not found.";
                }
                else
                {
                    response.Data = _mapper.Map<GetVideoDto>(video);
                }
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<ServiceResponse<List<GetVideoDto>>> GetVideos(int galleryId)
        {
            var response = new ServiceResponse<List<GetVideoDto>>();

            try
            {
                var videos = await _context.Videos.Where(v => v.Gallery.Id == galleryId)
                    .Include(v => v.Gallery).ToListAsync();

                if (videos.Count == 0)
                {
                    response.Success = false;
                    response.Message = "No Videos found.";
                }
                else
                {
                    response.Data = _mapper.Map<List<GetVideoDto>>(videos);
                }
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<GetVideoDto>> UpdateVideo(JsonPatchDocument<Models.Video> videoUpdates, int id)
        {
            var response = new ServiceResponse<GetVideoDto>();
            try
            {
                var video = await _context.Videos.Include(v => v.Gallery.User).FirstOrDefaultAsync(v => v.Id == id);

                if (GetUserRole().ToLower().Equals("admin") && video.Gallery.User.Id == GetUserID()) {

                    if (video == null)
                    {
                        response.Success = false;
                        response.Message = "Video not found.";
                    }
                    else
                    {
                        videoUpdates.ApplyTo(video);
                        await _context.SaveChangesAsync();
                        response.Data = _mapper.Map<GetVideoDto>(video);
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "You are not authorized to delete this video.";
                }
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
            
        }

        public async Task<ServiceResponse<bool>> DeleteVideo(int id)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var video = await _context.Videos.Include(v => v.Gallery.User).FirstOrDefaultAsync(v => v.Id == id);

                if (GetUserRole().ToLower().Equals("admin") && video.Gallery.User.Id == GetUserID()) {

                    if (video == null)
                    {
                        response.Success = false;
                        response.Message = "Video not found.";
                    }
                    else
                    {
                        string filePath = video.Uri;
                        File.Delete(filePath);

                        _context.Videos.Remove(video);
                        await _context.SaveChangesAsync();

                        response.Data = true;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "You are not authorized to delete this video.";
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