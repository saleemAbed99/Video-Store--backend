using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VideoStore_Backend.Dtos.Video;
using VideoStore_Backend.Models;
using VideoStore_Backend.Services.Video;

namespace VideoStore_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost("UploadVideo"), DisableRequestSizeLimit]
        public async Task<ActionResult<ServiceResponse<GetVideoDto>>> UploadVideo([FromForm] UploadVideoDto video)
        {
            var response = await _videoService.UploadVideo(video);
            if(response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("GetVideo/{id}")]
        public async Task<ActionResult<ServiceResponse<GetVideoDto>>> GetVideo(int id)
        {
            var response = await _videoService.GetVideo(id);
            if(response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("GetVideos/{galleryId}")]
        public async Task<ActionResult<ServiceResponse<List<GetVideoDto>>>> GetVideos(int galleryId)
        {
            var response = await _videoService.GetVideos(galleryId);
            if(response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("DeleteVideo/{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteVideo(int id)
        {
            var response = await _videoService.DeleteVideo(id);
            if(response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPatch("UpdateVideo/{id}")]
        public async Task<ActionResult<ServiceResponse<GetVideoDto>>> UpdateVideo(int id, [FromBody] JsonPatchDocument<Video> videoUpdates)
        {
            var response = await _videoService.UpdateVideo(videoUpdates, id);
            if(response.Success)
                return Ok(response);
            return BadRequest(response);
        }
    }
}