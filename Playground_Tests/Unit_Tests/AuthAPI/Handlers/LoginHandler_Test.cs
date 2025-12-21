using AuthAPI.Data.Entities;
using AuthAPI.Features.Commands.Login;
using AuthAPI.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Playground_Tests.Unit_Tests.AuthAPI.Mocks;
using Shared.Models.Common;
using Shared.Models.Requests.Auth;
using AuthAPI.Settings;
using Microsoft.Extensions.Options;
using Shared.Models.Responses.Auth;
using Microsoft.Extensions.Azure;

namespace Playground_Tests.Unit_Tests.AuthAPI.Handlers
{
    public class LoginHandler_Test
    {
        [Fact]
        public async Task ReturnsOk()
        {
            var mockUserManager = UserManagerMock.GetMock();
            var mockRepo = AutoRepoMock.GetMock();
            var mockLogger = NullLogger<LoginCommandHandler>.Instance;
            var mockMapper = new Mock<IMapper>();
            var authSettings = Options.Create(AuthSettingsProvider.GetSettings());

            string username = "Existing_User";

            var loginRequest = new LoginRequest
            {
                Username = username,
                Password = "Test123!"
            };

            var loginCommand = new LoginCommand(loginRequest);

            var mappedUser = new AppUser { UserName = loginRequest.Username };

            mockMapper.Setup(m => m.Map<AppUser>(loginRequest)).Returns(mappedUser);
            mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(true);

            var tokenService = new TokenService(
                mockRepo.Object,
                authSettings, 
                mockUserManager.Object);

            var loginHandler = new LoginCommandHandler(
                mockUserManager.Object,
                tokenService,
                mockLogger
             );

            var response = await loginHandler.Handle(loginCommand, new CancellationToken());

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Value.Should().NotBeNull();
            response.Value.Should().BeOfType<AuthResponse>();
            response.Value.Token.Should().NotBeNull();
            response.Value.RefreshToken.Should().NotBeNull();
            response.Value.Username.Should().Be(username);
        }

        [Fact]
        public async Task ReturnsError_WhenPasswordWrong()
        {
            var mockUserManager = UserManagerMock.GetMock();
            var mockRepo = AutoRepoMock.GetMock();
            var mockLogger = NullLogger<LoginCommandHandler>.Instance;
            var mockMapper = new Mock<IMapper>();
            var authSettings = Options.Create(AuthSettingsProvider.GetSettings());

            string username = "Existing_User";

            var loginDto = new LoginRequest
            {
                Username = username,
                Password = "Wrong_Password"
            };

            var loginCommand = new LoginCommand(loginDto);

            var mappedUser = new AppUser { UserName = loginDto.Username };

            mockMapper.Setup(m => m.Map<AppUser>(loginDto)).Returns(mappedUser);
            mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(false);

            var tokenService = new TokenService(
                mockRepo.Object,
                authSettings,
                mockUserManager.Object);

            var loginHandler = new LoginCommandHandler(
                mockUserManager.Object,
                tokenService,
                mockLogger
             );

            var response = await loginHandler.Handle(loginCommand, new CancellationToken());

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.Error.Type.Should().Be(HandlerErrorType.NotFound);
        }
    }
}
