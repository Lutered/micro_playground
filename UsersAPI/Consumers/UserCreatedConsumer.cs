using AutoMapper;
using Contracts.Requests.User;
using MassTransit;
using UsersAPI.Data;
using UsersAPI.DTOs;
using UsersAPI.Interfaces.Repositories;
using Contracts.Responses;

namespace UsersAPI.Consumers
{
    public class UserCreatedConsumer(IMapper _mapper, IUserRepository _userRepository): IConsumer<UserCreated>
    {
        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var contract = context.Message;

            var user = _mapper.Map<UserCreated, AppUserDTO>(contract);

            await _userRepository.CreateUserAsync(user);

            //await context.RespondAsync<ISuccessHandle>(new
            //{
            //    IsSuccessful = true
            //});
        }
    }
}
