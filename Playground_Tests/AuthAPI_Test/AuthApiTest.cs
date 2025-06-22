using AuthAPI.Controllers;
using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Services;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MockQueryable;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Playground_Tests.AuthAPI_Test
{
    public class AuthApiTest
    {
        private Mock<UserManager<AppUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<AppUser>>();
            return new Mock<UserManager<AppUser>>(
                store.Object,  null, null, null, null, null, null, null, null
            );
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsCreated()
        {
            // Arrange
            var mockUserManager = GetMockUserManager();
            var mockMapper = new Mock<IMapper>();

            string username = "TestUser";

            var registerDto = new RegisterDTO
            {
                Username = username,
                Password = "Test123!"
            };

            var mappedUser = new AppUser { UserName = registerDto.Username };

            var existingUsers = new AppUser[]
            {
                new AppUser { UserName = "existsingUser" }
            };

            var configData = new Dictionary<string, string?> {
                {"TokenKey", Config.Token}
            };

            IConfiguration configuration = new ConfigurationBuilder()
               .AddInMemoryCollection(configData)
               .Build();

            mockMapper.Setup(m => m.Map<AppUser>(registerDto)).Returns(mappedUser);

            mockUserManager
                .Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(um => um.Users)
                .Returns(existingUsers.BuildMock());

            var tokenService = new TokenService(configuration, mockUserManager.Object);

            var controller = new AuthController(mockUserManager.Object, mockMapper.Object, tokenService);

            // Act
            var response = await controller.Register(registerDto);

            //// Assert
            Assert.NotNull(response.Result);

            var result = (ObjectResult)response.Result;

            Assert.Equal(result.StatusCode, 201);
            Assert.NotNull(result.Value);

            var user = Assert.IsType<UserDTO>(result.Value);

            Assert.NotEmpty(user.Token);
            Assert.Equal(user.Username, username.ToLower());
        }
    }
}
