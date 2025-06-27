using AuthAPI.Controllers;
using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Services;
using AutoMapper;
using Contracts;
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
    public class RegisterTest
    {
        private Mock<UserManager<AppUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object,  null, null, null, null, null, null, null, null
            );

            var existingUsers = new AppUser[]
            {
                new AppUser { UserName = "existsinguser" }
            };

            mockUserManager
               .Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
               .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(um => um.Users)
                .Returns(existingUsers.BuildMock());

            return mockUserManager;
        }

        private IConfiguration GetConfiguration()
        {
            var configData = new Dictionary<string, string?> {
                {"TokenPath", Config.TokenPath}
            };

            IConfiguration configuration = new ConfigurationBuilder()
               .AddInMemoryCollection(configData)
               .Build();

            return configuration;
        }

        private Mock<IPublishEndpoint> GetPublisherMock()
        {
            var mockPublisher = new Mock<IPublishEndpoint>();

            mockPublisher
            .Setup(x => x.Publish<UserCreated>(
                It.IsAny<UserCreated>(), 
                It.IsAny<CancellationToken>()
              ));

            return mockPublisher;
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsCreated()
        {
            var configuration = GetConfiguration();

            var mockUserManager = GetMockUserManager();
            var mockPublisher = GetPublisherMock();
            var mockMapper = new Mock<IMapper>();

            string username = "TestUser";

            var registerDto = new RegisterDTO
            {
                Username = username,
                Password = "Test123!"
            };

            var mappedUser = new AppUser { UserName = registerDto.Username };

            mockMapper.Setup(m => m.Map<AppUser>(registerDto)).Returns(mappedUser);

            var tokenService = new TokenService(configuration, mockUserManager.Object);

            var controller = new AuthController(
                mockUserManager.Object, 
                mockMapper.Object, 
                tokenService, 
                mockPublisher.Object);

            var response = await controller.Register(registerDto);

            Assert.NotNull(response.Result);

            var result = (ObjectResult)response.Result;

            Assert.Equal(result.StatusCode, 201);
            Assert.NotNull(result.Value);

            var user = Assert.IsType<UserDTO>(result.Value);

            Assert.NotEmpty(user.Token);
            Assert.Equal(user.Username, username.ToLower());
        }

        [Fact]
        public async Task Register_ReturnsError_WhenUserNameExists()
        {
            var configuration = GetConfiguration();

            var mockUserManager = GetMockUserManager();
            var mockPublisher = GetPublisherMock();
            var mockMapper = new Mock<IMapper>();

            string username = "existsinguser";

            var registerDto = new RegisterDTO
            {
                Username = username,
                Password = "Test123!"
            };

            var mappedUser = new AppUser { UserName = registerDto.Username };


            mockMapper.Setup(m => m.Map<AppUser>(registerDto)).Returns(mappedUser);

            var tokenService = new TokenService(configuration, mockUserManager.Object);

            var controller = new AuthController(
                mockUserManager.Object, 
                mockMapper.Object, 
                tokenService, 
                mockPublisher.Object);

            var response = await controller.Register(registerDto);

            Assert.NotNull(response.Result);

            var result = (ObjectResult)response.Result;

            Assert.Equal(result.StatusCode, 400);
        }
    }
}
