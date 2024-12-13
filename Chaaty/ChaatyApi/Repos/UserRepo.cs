using AutoMapper;
using AutoMapper.QueryableExtensions;
using ChaatyApi.Data;
using ChaatyApi.DTOs;
using ChaatyApi.Entities;
using ChaatyApi.Helpers;
using ChaatyApi.Params;
using ChaatyApi.Repos.interfaces;
using ChaatyApi.Services.interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace ChaatyApi.Repos
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IPhotoService photoService;

        public UserRepo(ApplicationDbContext context, IMapper mapper, IPhotoService photoService)
        {
            this.context = context;
            this.mapper = mapper;
            this.photoService = photoService;
        }
       
        public async Task<PagedList<UserDto>> GetUsersAsync(UserFilterParams userParams ,string userId)
        {
            var users = context.Users
                .Include(i => i.Photos)
                .Include(l => l.Languages)
                .Where(i => i.Id != userId)
                .ProjectTo<UserDto>(mapper.ConfigurationProvider).AsQueryable();

            if (userParams.MinAge != null && userParams.MaxAge != null)
            {
                users = users.Where(a => a.Age >= userParams.MinAge && a.Age <= userParams.MaxAge);
            }

            // Apply the language filter
            if (!string.IsNullOrEmpty(userParams.LanguageName))
            {
                users = users.Where(u => u.Languages.Any(l => l.LanguageName == userParams.LanguageName));
            }

            // Apply the country filter
            if (!string.IsNullOrEmpty(userParams.Country))
            {
                users = users.Where(u => u.Country == userParams.Country);
            }

            var pagedList= await PagedList<UserDto>.CreateAsync(users, userParams.PageNumber,userParams.PageSize);
            return pagedList;
        }
        public async Task<UserDetailsDto> GetByIdAsync(string id, string currentUserID)
        {
            var user = await context.Users.Include(p => p.Photos).
                ProjectTo<UserDetailsDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(i => i.Id == id);

            var friendshipStatus = await context.Friendships
                       .Where(x => (x.SenderUserId == currentUserID && x.ReceiverUserId == id) ||
                                   (x.SenderUserId == id && x.ReceiverUserId == currentUserID))
                       .Select(x => new { x.SenderUserId, x.FriendshipStatus,x.Id })
                       .FirstOrDefaultAsync();
            if (friendshipStatus != null)
            {
                user.isThereFriendRequest = true;
                user.IsSender=friendshipStatus.SenderUserId == currentUserID;
                user.FriendshipStatus=friendshipStatus.FriendshipStatus;
                user.FriendShipId = friendshipStatus.Id;

            }

            return user;
        }  
        
        public async Task<UserDetailsDto> GetCurrentUserDetails(string id)
        {
            var user = await context.Users.Include(p => p.Photos).
                ProjectTo<UserDetailsDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(i => i.Id == id);
            return user;
        }  

        public async Task<AppUser> GetByIdAsyncLight(string id)
        {
            var user = await context.Users.FindAsync(id);
            return user;
        }


        public async Task<IEnumerable<UserDto>> GetByNameAsync(string name)
        {
            var user = await context.Users.
                ProjectTo<UserDto>(mapper.ConfigurationProvider)
                .Where(x => x.FirstName == name || x.LastName == name).
                ToListAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(UserToUpdateDto user, string userId)
        {

            try
            {
                var userToUpdate = await context.Users.FirstOrDefaultAsync(i => i.Id == userId);

                if (userToUpdate == null)
                {
                    return false;
                }

                mapper.Map(user, userToUpdate);
                context.Users.Update(userToUpdate);
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<PhotoToRreturnDto> UploadPhoto(IFormFile file, string userID)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "File cannot be null");
            }

            var user = await context.Users.Include(i=>i.Photos)
                .FirstOrDefaultAsync(i => i.Id == userID);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var result = await photoService.UploadImageAsync(file);
            if (result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                IsMain = user.Photos.Count == 0
            };

            user.Photos.Add(photo);
            await context.SaveChangesAsync();

            return mapper.Map<PhotoToRreturnDto>(photo);
        }

        public async Task UpdateMainPhoto(int photoId, string userId)
        {
            var user = await context.Users.Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

        
            var currentMainPhoto = user.Photos.SingleOrDefault(photo => photo.IsMain);

            var newMainPhoto = user.Photos.SingleOrDefault(photo => photo.Id == photoId);

          
            if (newMainPhoto == null)
            {
                throw new Exception("Photo not found");
            }

            if (currentMainPhoto != null)
            {
                currentMainPhoto.IsMain = false;
            }
            newMainPhoto.IsMain = true;

            await context.SaveChangesAsync();
        }
        public async Task DeletePhoto(int photoId, string userId)
        {
            var user = await context.Users.Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }


            var PhotoToDelete = user.Photos.SingleOrDefault(photo => photo.Id == photoId);

            if (PhotoToDelete == null || PhotoToDelete.IsMain)
            {
                throw new Exception("Photo not found");
            }

            var result= await photoService.DeleteImageAsync(PhotoToDelete.PublicId);
            if(result.Error != null)
            {
                throw new Exception(result.Error.Message);
            }

            user.Photos.Remove(PhotoToDelete);
            await context.SaveChangesAsync();
        }
        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task<AppUser> GetByIdAsyncIncludigPhotos(string id)
        {
            var user = await context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(i=>i.Id==id);
            return user;
        }   
        public async Task<UserDto> GetByIdAsyncIncludigPhotos2(string id)
        {
            var user = await context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(i=>i.Id==id);
            return mapper.Map<UserDto>(user);
        }
    }

}
