using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VideoStore_Backend.Dtos.Gallery;
using VideoStore_Backend.Models;
using VideoStore_Backend.Services.Gallery;

namespace VideoStore_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GalleryController : ControllerBase
    {
        private readonly IGalleryService _galleryService;
        public GalleryController(IGalleryService galleryService)
        {
            _galleryService = galleryService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetGalleryDto>>> GetGallery(int id)
        {
            var response = await _galleryService.GetGallery(id);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("ListGalleries")]
        public async Task<ActionResult<ServiceResponse<List<GetGalleryDto>>>> ListGalleries()
        {
            var response = await _galleryService.ListGalleries();
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpGet("ListAdminGalleries")]
        public async Task<ActionResult<ServiceResponse<List<GetGalleryDto>>>> ListAdminGalleries()
        {
            var response = await _galleryService.ListAdminGalleries();
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("CreateGallery")]
        public async Task<ActionResult<ServiceResponse<GetGalleryDto>>> CreateGallery(CreateGalleryDto gallery)
        {
            var response = await _galleryService.CreateGallery(gallery);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult<ServiceResponse<GetGalleryDto>>> UpdateGallery(JsonPatchDocument<Gallery> galleryUpdates, int id)     
        {
            var response = await _galleryService.UpdateGallery(galleryUpdates, id);
            if(response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteGallery(int id)
        {
            var response = await _galleryService.DeleteGallery(id);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

    }
}