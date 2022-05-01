using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VideoStore_Backend.Auth;
using VideoStore_Backend.Dtos.User;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo)
        {
            _authRepo = authRepo;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login([FromBody] LoginDto request)
        {
            var response = await _authRepo.Login(request.Username, request.Password);

            if(!response.Success) 
                return BadRequest(response);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register([FromBody] RegisterDto user) 
        {
            var response = await _authRepo.Register(
                new User {
                    Username = user.Username,
                    DOB = user.DOB.Date,
                    Gender = user.Gender,
                    Role = user.Role
                },
                user.password
            );
        
            if(!response.Success) 
                return BadRequest(response);

            return Ok(response);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> GetUser(int id)
        {
            var response = await _authRepo.GetUser(id);
            if(!response.Success) 
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPatch("Update")]
        public async Task<ActionResult<ServiceResponse<GetUserDto>>> UpdateUser(JsonPatchDocument<User> userUpdates)
        {
            var response = await _authRepo.UpdateUser(userUpdates);
            if(response.Success)
                return Ok(response);
            return BadRequest(response);
        }
    }
}