using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VideoStore_Backend.Dtos.Video;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Services.Video
{
    public interface IVideoService
    {
         public Task<ServiceResponse<GetVideoDto>> UploadVideo(UploadVideoDto video);
         public Task<ServiceResponse<GetVideoDto>> GetVideo(int id);
         public Task<ServiceResponse<List<GetVideoDto>>> GetVideos(int galleryId);
         public Task<ServiceResponse<GetVideoDto>> UpdateVideo(JsonPatchDocument<Models.Video> videoUpdates, int id);
         public Task<ServiceResponse<bool>> DeleteVideo(int id);
    }
}