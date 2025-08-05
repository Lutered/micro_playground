using AuthAPI.Controllers;
using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Infrastructure.Handlers;
using AuthAPI.Intrefaces;
using AuthAPI.Mediator.Commands;
using AuthAPI.Services;
using AutoMapper;
using FluentAssertions;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MockQueryable;
using Moq;
using Playground_Tests.AuthAPI;
using Playground_Tests.Unit_Tests.AuthAPI.Mocks;
using Shared.Contracts;

namespace Playground_Tests.Unit_Tests.AuthAPI.Handlers
{
    public class RegisterHandler_Test
    {
        [Fact]
        public async Task ReturnsOk_WhenUserIsCreated()
        {
            var configuration = GetConfiguration();

            var mockUserManager = GetMockUserManager();
            var mockPublisher = GetPublisherMock();
            var mockRepo = GetMockAuthRepo();
            var mockLogger = NullLogger<RegisterHandler>.Instance;
            var mockMapper = new Mock<IMapper>();
   
            string username = "TestUser";

            var registerDto = new RegisterDTO
            {
                Username = username,
                Password = "Test123!"
            };

            var registerCommand = new RegisterCommand(registerDto);

            var mappedUser = new AppUser { UserName = username };

            mockMapper.Setup(m => m.Map<AppUser>(registerDto)).Returns(mappedUser);

            var tokenService = new TokenService(mockRepo.Object, configuration, mockUserManager.Object);

            var registerHandler = new RegisterHandler(
                mockUserManager.Object,
                mockMapper.Object,
                tokenService,
                mockPublisher.Object,
                mockLogger
            );

            var response = await registerHandler.Handle(registerCommand, new CancellationToken());

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Value.Should().NotBeNull();
            response.Value.Should().BeOfType<AuthResponseDTO>();
            response.Value.Token.Should().NotBeNull();
            response.Value.RefreshToken.Should().NotBeNull();
            response.Value.Username.Should().Be(username);
        }

        [Fact]
        public async Task ReturnsError_WhenUserNameExists()
        {
            var configuration = GetConfiguration();

            var mockUserManager = GetMockUserManager();
            var mockPublisher = GetPublisherMock();
            var mockRepo = GetMockAuthRepo();
            var mockLogger = NullLogger<RegisterHandler>.Instance;
           // var mockLogger = LoggerMock.GetMock<RegisterHandler>();
            var mockMapper = new Mock<IMapper>();

            string username = "Existing_User";

            var registerDto = new RegisterDTO
            {
                Username = username,
                Password = "Test123!"
            };

            var registerCommand = new RegisterCommand(registerDto);

            var mappedUser = new AppUser { UserName = registerDto.Username };

            mockMapper.Setup(m => m.Map<AppUser>(registerDto)).Returns(mappedUser);

            var tokenService = new TokenService(mockRepo.Object, configuration, mockUserManager.Object);

            var registerHandler = new RegisterHandler(
                mockUserManager.Object,
                mockMapper.Object,
                tokenService,
                mockPublisher.Object,
                mockLogger
            );

            var response = await registerHandler.Handle(registerCommand, new CancellationToken());

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
        }

        private Mock<IAuthRepository> GetMockAuthRepo()
        {
            var mockAuthRepo = new Mock<IAuthRepository>();

            mockAuthRepo
              .Setup(r => r.AddRefreshToken(It.IsAny<RefreshToken>()));

            mockAuthRepo
                .Setup(r => r.SaveChangesAsync());

            return mockAuthRepo;
        }
        private Mock<UserManager<AppUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            var existingUsers = new AppUser[]
            {
                new AppUser { UserName = "Existing_User" }
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
                {"Authorization:SecretKeyPath", Config.TokenPath}
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
    }
}
