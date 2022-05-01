using System.Threading.Tasks;
using VideoStore_Backend.Dtos.FavoriteList;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Services.FavoriteList
{
    public interface IFavoriteListService
    {
         Task<bool> CreateFavoriteList(CreateFavoriteListDto favoriteList);
         Task<ServiceResponse<GetFavoriteListDto>> GetFavoriteList();
         Task<ServiceResponse<GetFavoriteListDto>> AddVideoToFavoriteList(HandleFavoriteListDto input);
         Task<ServiceResponse<bool>> DeleteVideoFromFavoriteList(int id);
    }
}