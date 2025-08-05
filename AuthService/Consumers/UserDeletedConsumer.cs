using AuthAPI.Data.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts;

namespace AuthAPI.Consumers
{
    public class UserDeletedConsumer(UserManager<AppUser> userManager) : IConsumer<UserDeleted>
    {
        public async Task Consume(ConsumeContext<UserDeleted> context)
        {
            var contract = context.Message;

            var user = await userManager.Users
                    .FirstOrDefaultAsync(x => x.UserName == contract.Username.ToLower());

            if (user == null) return;

            await userManager.DeleteAsync(user);
        }
    }
}
