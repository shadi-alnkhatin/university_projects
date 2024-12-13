using ChaatyApi.DTOs;
using ChaatyApi.Helpers;
using ChaatyApi.Repos.interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChaatyApi.Controllers
{
    [ApiController]
    [Route("friendship")]
    [Authorize]
    [ServiceFilter(typeof(LastActiveFilter))]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipRepo friendshipRepo;

        public FriendshipController(IFriendshipRepo friendshipRepo)
        {
            this.friendshipRepo = friendshipRepo;
        }

        [HttpGet("requests")]
        public async Task<ActionResult<IEnumerable<FriendshipDto>>> GetFriendshipRequestAsync()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return BadRequest();
            }
            var friendshipRequests = await friendshipRepo.GetFriendsRequestAsync(userId);
            return Ok(friendshipRequests);
        }
       [HttpGet("my-friends")]
        public async Task<ActionResult<IEnumerable<FriendshipDto>>> GetFriendsAsync()
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
            {
                return BadRequest();
            }
           var friends= await friendshipRepo.GetFriendsAsync(userId);

            return Ok(friends);
        }

        [HttpPost("send-request/{receiverId}")]
        public async Task<IActionResult> SendFriendshipRequest(string receiverId)
        {
           var senderId = User.Identity.GetUserId();

            if(senderId==null || receiverId==senderId )
            {
                return BadRequest();
            }
           
            await friendshipRepo.SendFriendRequestAsync(senderId, receiverId);
            return Ok();
        }


        [HttpPost("accept-request/{friendshipId}")]
        public async Task<IActionResult> AcceptFriendRequestAsync(int friendshipId)
        {
            if(!await friendshipRepo.IsThereRequestFriendRequest(friendshipId))
            {
                return BadRequest();
            }
            await friendshipRepo.AcceptFriendRequestAsync(friendshipId);
            return Created();
        }


        [HttpDelete("reject-request/{friendshipId}")]
        public async Task<IActionResult> RejectFriendshipRequest(int friendshipId)
        {
            if (!await friendshipRepo.IsThereRequestFriendRequest(friendshipId))
            {
                return BadRequest();
            }
            await friendshipRepo.RejectFriendRequestAsync(friendshipId);
            return Ok();
        }

    }
}
