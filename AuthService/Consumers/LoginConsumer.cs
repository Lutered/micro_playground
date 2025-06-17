using AuthAPI.Data.Entities;
using AuthAPI.Intrefaces;
using Contracts.AuthApi.Requests;
using Contracts.AuthApi.Responses;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Consumers
{
    public class LoginConsumer(IAuthService _authService) : IConsumer<Login>
    {
        public async Task Consume(ConsumeContext<Login> context)
        {
            var loginContract = context.Message;

            var response = await _authService.LoginAsync(loginContract);

            await context.RespondAsync<UserResponse>(response);
        }
    }
}
