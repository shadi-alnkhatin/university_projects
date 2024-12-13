using ChaatyApi.Data;
using ChaatyApi.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChaatyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuggyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BuggyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {  
          return NotFound(); 
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            return StatusCode(500);
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest();
        }
    }
}
