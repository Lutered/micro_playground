using AuthAPI.Controllers;
using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Services;
using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MockQueryable;
using Moq;

namespace Playground_Tests.AuthAPI_Test
{
    public class LoginTest
    {
        private Mock<UserManager<AppUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            var existingUsers = new AppUser[]
            {
                new AppUser { UserName = "existsinguser" }
            };

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
            .Setup(x => x.Publish<UserLogin>(
                It.IsAny<UserLogin>(),
                It.IsAny<CancellationToken>()
              ));

            return mockPublisher;
        }

        [Fact]
        public async Task Login_ReturnsOk()
        {
            var configuration = GetConfiguration();

            var mockUserManager = GetMockUserManager();
            var mockPublisher = GetPublisherMock();
            var mockMapper = new Mock<IMapper>();

            string username = "existsinguser";

            var loginDto = new LoginDTO
            {
                Username = username,
                Password = "Test123!"
            };

            var mappedUser = new AppUser { UserName = loginDto.Username };

            mockMapper.Setup(m => m.Map<AppUser>(loginDto)).Returns(mappedUser);
            mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(true);

            var tokenService = new TokenService(configuration, mockUserManager.Object);

            var controller = new AuthController(
                mockUserManager.Object, 
                mockMapper.Object, 
                tokenService, 
                mockPublisher.Object);

            var response = await controller.Login(loginDto);

            Assert.NotNull(response.Result);

            var result = (ObjectResult)response.Result;

            Assert.Equal(result.StatusCode, 200);
            Assert.NotNull(result.Value);

            var user = Assert.IsType<AuthUserDTO>(result.Value);

            Assert.NotEmpty(user.Token);
        }
    }
}
