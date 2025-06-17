using AuthAPI.Data.Entities;
using AuthAPI.Intrefaces;
using Contracts.AuthApi.Requests;
using Contracts.AuthApi.Responses;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Consumers
{
    public class RegisterConsumer(IAuthService _authService) : IConsumer<Register>
    {
        public async Task Consume(ConsumeContext<Register> context)
        {
            var registerContract = context.Message;

            var response = await _authService.RegisterAsync(registerContract);

            await context.RespondAsync<UserResponse>(response);
        }
    }
}
