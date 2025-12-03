using AuthAPI.Data.Entities;
using AuthAPI.Models;
using AuthAPI.Features.Commands.Login;
using AuthAPI.Mediator.Commands;
using AuthAPI.MediatR.Commands;
using AuthAPI.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Playground_Tests.Unit_Tests.AuthAPI.Mocks;
using Shared.Models.Common;

namespace Playground_Tests.Unit_Tests.AuthAPI.Handlers
{
    public class LoginHandler_Test
    {
        [Fact]
        public async Task ReturnsOk()
        {
            var configuration = ConfigurationMock.GetMock();

            var mockUserManager = UserManagerMock.GetMock();
            var mockRepo = AutoRepoMock.GetMock();
            var mockLogger = NullLogger<LoginCommandHandler>.Instance;
            var mockMapper = new Mock<IMapper>();

            string username = "Existing_User";

            var loginDto = new LoginDTO
            {
                Username = username,
                Password = "Test123!"
            };

            var loginCommand = new LoginCommand(loginDto);

            var mappedUser = new AppUser { UserName = loginDto.Username };

            mockMapper.Setup(m => m.Map<AppUser>(loginDto)).Returns(mappedUser);
            mockUserManager.Setup(um => um.CheckPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>())).ReturnsAsync(true);

            var tokenService = new TokenService(
                mockRepo.Object,
                configuration, 
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
            response.Value.Should().BeOfType<AuthResponseDTO>();
            response.Value.Token.Should().NotBeNull();
            response.Value.RefreshToken.Should().NotBeNull();
            response.Value.Username.Should().Be(username);
        }

        [Fact]
        public async Task ReturnsError_WhenPasswordWrong()
        {
            var configuration = ConfigurationMock.GetMock();

            var mockUserManager = UserManagerMock.GetMock();
            var mockRepo = AutoRepoMock.GetMock();
            var mockLogger = NullLogger<LoginCommandHandler>.Instance;
            var mockMapper = new Mock<IMapper>();

            string username = "Existing_User";

            var loginDto = new LoginDTO
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
                configuration,
                mockUserManager.Object);

            var loginHandler = new LoginCommandHandler(
                mockUserManager.Object,
                tokenService,
                mockLogger
             );

            var response = await loginHandler.Handle(loginCommand, new CancellationToken());

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeFalse();
            response.Error.Type.Should().Be(HandlerErrorType.Unauthorized);
        }
    }
}
