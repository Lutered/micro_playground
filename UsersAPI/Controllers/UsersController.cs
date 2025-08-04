using AutoMapper;
using Contracts.Requests.User;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersAPI.DTOs;
using UsersAPI.Helpers;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<UsersController> _logger;
        public UsersController(
            IUserRepository userRepository, 
            IPublishEndpoint publishEndpoint,
            ILogger<UsersController> logger) 
        {
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        [HttpGet]
        [Route("get/{username}")]
        public async Task<ActionResult<AppUserDTO>> GetUser(string username)
        {
            var user = await _userRepository.GetUserAsync(username);

            if(user == null) { return NotFound("User was not found"); }

            return user;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ActionResult<PagedList<AppUserDTO>>> GetUsers([FromQuery]PageDTO pageParams)
        {
            return await _userRepository.GetUsersAsync(pageParams.Page, pageParams.PageSize);
        }

        //[HttpPost]
        //[Route("create")]
        //public async Task<ActionResult> CreateUser(AppUserDTO appUser)
        //{
        //    try
        //    {
        //        await _userRepository.CreateUserAsync(appUser);
        //        return Ok();
        //    }
        //    catch (Exception ex) 
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateUser(AppUserDTO appUser)
        {
            bool result = await _userRepository.UpdateUserAsync(appUser);

            return result ? Ok() : NotFound();
        }

        [HttpDelete]
        [Route("delete/{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            await _userRepository.DeleteUserAsync(username);

            await _publishEndpoint.Publish<UserDeleted>(new UserDeleted
            {
                Username = username
            });

            return Ok();
        }
    }
}
