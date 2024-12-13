using ChaatyApi.DTOs;
using ChaatyApi.Entities;
using ChaatyApi.Helpers;
using ChaatyApi.Params;

namespace ChaatyApi.Repos.interfaces
{
    public interface IUserRepo
    {
        Task<PagedList<UserDto>> GetUsersAsync(UserFilterParams userFilterParams, string userId);

        Task<IEnumerable<UserDto>> GetByNameAsync(string name);
        Task<UserDetailsDto> GetByIdAsync(string id, string currentUserID);
        Task<AppUser> GetByIdAsyncLight(string id);
        Task<AppUser> GetByIdAsyncIncludigPhotos(string id);
        Task<UserDto> GetByIdAsyncIncludigPhotos2(string id);

        Task<UserDetailsDto> GetCurrentUserDetails( string currentUserID);
        Task<bool> UpdateUserAsync(UserToUpdateDto user, string userId);
        Task<PhotoToRreturnDto> UploadPhoto(IFormFile file, string userID);
        Task UpdateMainPhoto(int photoId, string userId);
        Task DeletePhoto(int photoId, string userId);
        Task SaveChangesAsync();



    }
}
