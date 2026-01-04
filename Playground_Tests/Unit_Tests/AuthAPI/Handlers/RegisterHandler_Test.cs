using AuthAPI.Data.Entities;
using AuthAPI.Features.Commands.Register;
using AuthAPI.Services;
using AutoMapper;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Playground_Tests.Unit_Tests.AuthAPI.Mocks;
using Shared.Models.Contracts.User.Events;
using Shared.Models.Requests.Auth;
using Microsoft.Extensions.Options;
using Shared.Models.Responses.Auth;

namespace Playground_Tests.Unit_Tests.AuthAPI.Handlers
{
    public class RegisterHandler_Test
    {
        [Fact]
        public async Task ReturnsOk()
        {
            var mockUserManager = UserManagerMock.GetMock();
            var mockPublisher = GetPublisherMock();
            var mockRepo = AutoRepoMock.GetMock();
            var mockLogger = NullLogger<RegisterCommandHandler>.Instance;
            var mockMapper = new Mock<IMapper>();
            var authSettings = Options.Create(AuthSettingsProvider.GetSettings());

            string username = "TestUser";

            var registerDto = new RegisterRequest
            {
                Username = username,
                Password = "Test123!"
            };

            var registerCommand = new RegisterCommand(registerDto);

            var mappedUser = new AppUser { UserName = username };

            mockMapper.Setup(m => m.Map<AppUser>(registerDto)).Returns(mappedUser);

            var tokenService = new TokenService(mockRepo.Object, authSettings, mockUserManager.Object);

            var registerHandler = new RegisterCommandHandler(
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
            response.Value.Should().BeOfType<AuthResponse>();
            response.Value.Token.Should().NotBeNull();
            response.Value.RefreshToken.Should().NotBeNull();
            response.Value.Username.Should().Be(username);
        }

        [Fact]
        public async Task ReturnsError_WhenUserNameExists()
        {
            var mockUserManager = UserManagerMock.GetMock();
            var mockPublisher = GetPublisherMock();
            var mockRepo = AutoRepoMock.GetMock();
            var mockLogger = NullLogger<RegisterCommandHandler>.Instance;
            var mockMapper = new Mock<IMapper>();
            var authSettings = Options.Create(AuthSettingsProvider.GetSettings());

            string username = "Existing_User";

            var registerDto = new RegisterRequest
            {
                Username = username,
                Password = "Test123!"
            };

            var registerCommand = new RegisterCommand(registerDto);

            var mappedUser = new AppUser { UserName = registerDto.Username };

            mockMapper.Setup(m => m.Map<AppUser>(registerDto)).Returns(mappedUser);

            var tokenService = new TokenService(mockRepo.Object, authSettings, mockUserManager.Object);

            var registerHandler = new RegisterCommandHandler(
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
        private Mock<IPublishEndpoint> GetPublisherMock()
        {
            var mockPublisher = new Mock<IPublishEndpoint>();

            mockPublisher
            .Setup(x => x.Publish<UserCreatedEvent>(
                It.IsAny<UserCreatedEvent>(),
                It.IsAny<CancellationToken>()
              ));

            return mockPublisher;
        }
    }
}
