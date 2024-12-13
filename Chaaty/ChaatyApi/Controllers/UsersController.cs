using ChaatyApi.DTOs;
using ChaatyApi.Helpers;
using ChaatyApi.Params;
using ChaatyApi.Repos.interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChaatyApi.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]
    [ServiceFilter(typeof(LastActiveFilter))]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo userRepo;

        public UsersController(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }
        [HttpGet("all")]
        public async Task<IActionResult> All([FromQuery]UserFilterParams userFilterParams)
        {
            var userID=User.Identity.GetUserId();
            var users = await userRepo.GetUsersAsync(userFilterParams,userID);
            Response.AddPaginationHeader(users.CurrentPage,users.PageSize,
                users.TotalCount,users.TotalPages);
            return  Ok(users);
        }
       [HttpGet("detail/{id}")]
        public async Task<IActionResult> Detail( string id)
        {
            var userID = User.Identity.GetUserId();

            var user = await userRepo.GetByIdAsync(id ,userID);
            if(user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpGet("my-details")]
        public async Task<IActionResult> CurrentUserDetails()
        {
            var userID = User.Identity.GetUserId();

            var user = await userRepo.GetCurrentUserDetails(userID);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserToUpdateDto user)
        {
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is missing or invalid.");
            }
            if (await userRepo.UpdateUserAsync(user, userId))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPost("upload/photo")]

        public async Task<ActionResult<PhotoToRreturnDto>> UploadPhoto(IFormFile file)
        {
            var userId = User.Identity.GetUserId();
            if (file.Length > 0 && userId != null)
            {
                var photo = await userRepo.UploadPhoto(file, userId);
                return Ok(photo);
            }
            return BadRequest();
        }

        [HttpPut("update/main-photo/{photoId}")]
        public async Task<IActionResult> UpdateMainPhoto(int photoId)
        {
            var userId = User.Identity.GetUserId();
            if ( userId != null)
            {
                  await userRepo.UpdateMainPhoto(photoId, userId);
                return Created();
            }
            return BadRequest();
        }

        [HttpDelete("delete/photo/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var userId = User.Identity.GetUserId();
            if (userId != null)
            {
                await userRepo.DeletePhoto(photoId, userId);
                return Ok();
            }
            return BadRequest();
        }

    }
}
