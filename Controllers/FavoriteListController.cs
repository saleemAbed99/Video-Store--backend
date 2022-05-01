using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VideoStore_Backend.Dtos.FavoriteList;
using VideoStore_Backend.Models;
using VideoStore_Backend.Services.FavoriteList;

namespace VideoStore_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FavoriteListController : ControllerBase
    {
        private readonly IFavoriteListService _favoriteListService;

        public FavoriteListController(IFavoriteListService favoriteListService)
        {
            _favoriteListService = favoriteListService;
        }

        [HttpGet("GetFavoriteList")]
        public async Task<ActionResult<ServiceResponse<GetFavoriteListDto>>> GetFavoriteList()
        {
            var response = await _favoriteListService.GetFavoriteList();
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("AddVideoToFavoriteList")]
        public async Task<ActionResult<ServiceResponse<GetFavoriteListDto>>> AddVideoToFavoriteList(HandleFavoriteListDto input)
        {
            var response = await _favoriteListService.AddVideoToFavoriteList(input);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }

        [HttpDelete("DeleteVideoFromFavoriteList/{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteVideoFromFavoriteList(int id)
        {
            var response = await _favoriteListService.DeleteVideoFromFavoriteList(id);
            if (response.Success)
                return Ok(response);
            return BadRequest(response);
        }
    }
}