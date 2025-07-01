using AutoMapper;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using UsersAPI.Data;
using UsersAPI.Data.Entities;
using UsersAPI.DTOs;
using UsersAPI.Helpers;

namespace UsersAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UsersController(UserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("user/{username}")]
        public async Task<ActionResult<AppUserDTO>> GetUser(string username)
        {
            var user = await _userRepository.GetUserAsync(username);

            if(user == null) { return NotFound("User was not found"); }

            return user;
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<PagedList<AppUserDTO>>> GetUsers(PageDTO pageParams)
        {
            return await _userRepository.GetUsersAsync(pageParams.Page, pageParams.PageSize);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(AppUserDTO appUser)
        {
            try
            {
                await _userRepository.CreateUserAsync(appUser);
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser()
        {
            return Ok();
        }
    }
}
