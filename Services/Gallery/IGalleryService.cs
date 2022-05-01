using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using VideoStore_Backend.Dtos.Gallery;
using VideoStore_Backend.Models;

namespace VideoStore_Backend.Services.Gallery
{
    public interface IGalleryService
    {
        Task<ServiceResponse<GetGalleryDto>> CreateGallery(CreateGalleryDto gallery);
        Task<ServiceResponse<bool>> DeleteGallery(int id);
        Task<ServiceResponse<List<GetGalleryDto>>> ListGalleries();
        Task<ServiceResponse<List<GetGalleryDto>>> ListAdminGalleries();
        Task<ServiceResponse<GetGalleryWithVideosDto>> GetGallery(int id);
        Task<ServiceResponse<GetGalleryDto>> UpdateGallery(JsonPatchDocument<Models.Gallery> galleryUpdates, int id);         
    }
}