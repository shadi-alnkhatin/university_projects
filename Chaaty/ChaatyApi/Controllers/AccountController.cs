using AutoMapper;
using ChaatyApi.Data;
using ChaatyApi.DTOs;
using ChaatyApi.Entities;
using ChaatyApi.Helpers;
using ChaatyApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChaatyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly TokenServices tokenServices;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,TokenServices tokenServices,IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenServices = tokenServices;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RigesterDto appUser)
        {
            if (ModelState.IsValid)
            {
                var user = mapper.Map<AppUser>(appUser);

                var result = await userManager.CreateAsync(user, appUser.Password);
                if (result.Succeeded)
                {
                    var token = tokenServices.GenerateToken(user);
                    var userToReturn = mapper.Map<AuthResponse>(user);
                    userToReturn.Token = token;

                    await signInManager.SignInAsync(user, isPersistent: true);
                    return Ok(userToReturn );
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof(LastActiveFilter))]
        public async Task<IActionResult> login(LoginDto loginDto)
        {

            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var token = tokenServices.GenerateToken(user);
                var userToReturn = mapper.Map<AuthResponse>(user);
                userToReturn.Token = token;
                return Ok(userToReturn);
            }

            return Unauthorized(new Respone { isSucces=false,Errormessage="Your Email Or Password is invalid"});

        }

    }
}


