using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using VideoStore_Backend.Dtos.User;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Auth
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<ServiceResponse<GetUserDto>> GetUser(int id);
        Task<ServiceResponse<GetUserDto>> UpdateUser(JsonPatchDocument<User> userUpdates);
        Task<bool> UserExists(string username);
    }
}