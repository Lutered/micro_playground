using AutoMapper;
using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Serilog.Context;
using UsersAPI.Data;
using UsersAPI.Data.Entities;
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
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger) 
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("getuser/{username}")]
        public async Task<ActionResult<AppUserDTO>> GetUser(string username)
        {
            var user = await _userRepository.GetUserAsync(username);

            if(user == null) { return NotFound("User was not found"); }

            return user;
        }

        [HttpGet]
        [Route("getusers")]
        public async Task<ActionResult<PagedList<AppUserDTO>>> GetUsers([FromQuery]PageDTO pageParams)
        {
            return await _userRepository.GetUsersAsync(pageParams.Page, pageParams.PageSize);
        }

        [HttpPost]
        [Route("createuser")]
        public async Task<ActionResult> CreateUser(AppUserDTO appUser)
        {
            try
            {
                await _userRepository.CreateUserAsync(appUser);
               // LogContext.PushProperty("CorrelId", Guid.NewGuid().ToString());
                _logger.LogInformation("User was created");
                return Ok();
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteuser")]
        public async Task<ActionResult> DeleteUser()
        {
            return Ok();
        }
    }
}
