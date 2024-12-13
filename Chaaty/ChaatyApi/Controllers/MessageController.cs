using AutoMapper;
using ChaatyApi.DTOs;
using ChaatyApi.Entities;
using ChaatyApi.Helpers;
using ChaatyApi.Repos.interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChaatyApi.Controllers
{
    [ApiController]
    [Route("messages")]
    [Authorize]
    [ServiceFilter(typeof(LastActiveFilter))]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepo messageRepo;
        private readonly IUserRepo userRepo;
        private readonly IMapper mapper;

        public MessageController(IMessageRepo messageRepo, IUserRepo userRepo,IMapper mapper)
        {
            this.messageRepo = messageRepo;
            this.userRepo = userRepo;
            this.mapper = mapper;
        }
        [HttpPost("send")]
        public async Task<ActionResult<MessageDto>> SendMessage(CreateMessageDto createMessage)
        {
            var userID=User.Identity.GetUserId();
            var currentUser =await userRepo.GetByIdAsyncIncludigPhotos(userID);
            var recipient= await userRepo.GetByIdAsyncIncludigPhotos(createMessage.UserID);

            if(currentUser == null || recipient==null)
            {
                return BadRequest();
            }
            var message = new Message
            {
                Sender = currentUser,
                Recipient = recipient,
                Content = createMessage.content,
                SenderFirstName = currentUser.FirstName,
                RecipientFirstName = recipient.FirstName
            };
            messageRepo.AddMessage(message);
            if(await messageRepo.SaveAllAsync()) 
            {
                return Ok(mapper.Map<MessageDto>(message));
            }
            return BadRequest("failed to send message");
        }

        [HttpGet("my-messages")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetMessagesForUser()
        {
            var userId=User.Identity.GetUserId();

            var users = await messageRepo.GetUsersFromChatAsync(userId);
            if(users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("chat/{recipientId}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetTheMessageThread(string recipientId)
        {
            var userId = User.Identity.GetUserId();

            var messageThread = await messageRepo.GetMessageThreadAsync(userId, recipientId);
            return Ok(messageThread);
        }
    }
}
