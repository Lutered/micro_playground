using AuthAPI.Data.Entities;
using AuthAPI.Features.Commands.Login;
using AuthAPI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Playground_Tests.Unit_Tests.AuthAPI.Mocks;
using AuthAPI.Features.Commands.Refresh;
using Microsoft.Extensions.Options;
using Shared.Models.Responses.Auth;

namespace Playground_Tests.Unit_Tests.AuthAPI.Handlers
{
    public class RefreshHandler_Test
    {
        [Fact]
        public async Task ReturnsOk()
        {
            var configuration = ConfigurationMock.GetMock();

            var mockUserManager = UserManagerMock.GetMock();
            var mockRepo = AutoRepoMock.GetMock();
            var mockLogger = NullLogger<LoginCommandHandler>.Instance;
            var authSettings = Options.Create(AuthSettingsProvider.GetSettings());

            string refreshToken = "correct_token";

            var refreshCommand = new RefreshCommand(refreshToken);

            var tokenService = new TokenService(
                mockRepo.Object,
                authSettings,
                mockUserManager.Object);

            var refreshHandler = new RefreshCommandHandler(
                mockRepo.Object,
                tokenService,
                mockLogger
             );

            var response = await refreshHandler.Handle(refreshCommand, new CancellationToken());

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Value.Should().NotBeNull();
            response.Value.Should().BeOfType<AuthResponse>();
            response.Value.Token.Should().NotBeNull();
            response.Value.RefreshToken.Should().NotBeNull();
            response.Value.Username.Should().Be("Existing_User");
        }
    }
}
