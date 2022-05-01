using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VideoStore_Backend.Data;
using VideoStore_Backend.Dtos.FavoriteList;
using VideoStore_Backend.Dtos.User;
using VideoStore_Backend.Models;
using VideoStore_Backend.Services.FavoriteList;

namespace VideoStore_Backend.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFavoriteListService _favoriteList;

        public AuthRepository(DataContext context, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContext, IFavoriteListService favoriteList)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
            _httpContext = httpContext;
            _favoriteList = favoriteList;
        }

        private int GetUserID() => int.Parse(_httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower()));

            if(user == null) 
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) 
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }
    
        public async Task<ServiceResponse<GetUserDto>> GetUser(int id) 
        {
            var response = new ServiceResponse<GetUserDto>();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if(user == null) 
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else
            {
                response.Data = _mapper.Map<GetUserDto>(user);
            }

            return response;
        }

       public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();

            try
            {
         
                if(await UserExists(user.Username)) 
                {
                    response.Success = false;
                    response.Message = "User already exists.";
                    return response;
                }

                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);

                if(user.Role.ToLower().Equals("user")) 
                {
                    var userFavoriteList = new CreateFavoriteListDto { User = user };

                    bool favoriteListCreated = await _favoriteList.CreateFavoriteList(userFavoriteList);
                    
                    if(!favoriteListCreated) 
                    {
                        response.Success = false;
                        response.Message = "Favorite list could not be created.";
                        return response;
                    }
                }

                await _context.SaveChangesAsync();

                response.Data = user.Id;
                response.Message = user.Username + " successfully registered";

             }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

         public async Task<ServiceResponse<GetUserDto>> UpdateUser(JsonPatchDocument<User> userUpdates)
        {
            var response = new ServiceResponse<GetUserDto>();
            
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserID());

                if(user == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return response;
                }

                userUpdates.ApplyTo(user);

                await _context.SaveChangesAsync();
                
                response.Data = _mapper.Map<GetUserDto>(user);
            }
            catch(Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            

            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(x => x.Username.ToLower().Equals(username.ToLower())))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i]){
                        return false;
                    }
                }
                return true;
            }
        }
        
        private string CreateToken(User user) 
        {
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}