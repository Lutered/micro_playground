using AuthAPI.Data;
using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Intrefaces;
using AuthAPI.Services;
using AutoMapper;
using Contracts;
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public AuthController(
            UserManager<AppUser> userManager, 
            IMapper mapper, 
            ITokenService tokenService,
            IPublishEndpoint publishEndpoint
         ) 
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        [RequestTimeout(milliseconds: 100)]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username))
                return BadRequest($"User with username {registerDTO.Username} already exists");

            var user = _mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            //var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            //if (!roleResult.Succeeded)
            //    return BadRequest(roleResult.Errors);

            await _publishEndpoint.Publish<UserCreated>(new UserCreated() { Username = user.UserName });

            return StatusCode(201, new UserDTO()
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            });
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.Users
              .FirstOrDefaultAsync(x => x.UserName == loginDTO.Username);

            if (user == null) return Unauthorized("Invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!result) return Unauthorized("Invalid password");

            await _publishEndpoint.Publish<UserLogin>(new UserLogin() { Username = user.UserName, LoginTime = DateTime.Now });

            return StatusCode(200, new UserDTO()
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user)
            });
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}
