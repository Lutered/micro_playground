using AuthAPI.Data.Entities;
using AuthAPI.DTOs;
using AuthAPI.Infrastructure.Handlers;
using AuthAPI.MediatR.Commands;
using AuthAPI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Playground_Tests.Unit_Tests.AuthAPI.Mocks;

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
            var mockLogger = NullLogger<LoginHandler>.Instance;

            string refreshToken = "correct_token";

            var refreshCommand = new RefreshCommand(refreshToken);

            var tokenService = new TokenService(
                mockRepo.Object,
                configuration,
                mockUserManager.Object);

            var refreshHandler = new RefreshHandler(
                mockRepo.Object,
                tokenService,
                mockLogger
             );

            var response = await refreshHandler.Handle(refreshCommand, new CancellationToken());

            response.Should().NotBeNull();
            response.IsSuccess.Should().BeTrue();
            response.Value.Should().NotBeNull();
            response.Value.Should().BeOfType<AuthResponseDTO>();
            response.Value.Token.Should().NotBeNull();
            response.Value.RefreshToken.Should().NotBeNull();
            response.Value.Username.Should().Be("Existing_User");
        }
    }
}
