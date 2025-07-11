﻿using AutoMapper;
using Contracts;
using MassTransit;
using UsersAPI.Data;
using UsersAPI.DTOs;
using UsersAPI.Interfaces.Repositories;

namespace UsersAPI.Consumers
{
    public class UserCreatedConsumer(IMapper _mapper, IUserRepository _userRepository): IConsumer<UserCreated>
    {
        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var contract = context.Message;

            var user = _mapper.Map<UserCreated, AppUserDTO>(contract);

            await _userRepository.CreateUserAsync(user);
        }
    }
}
