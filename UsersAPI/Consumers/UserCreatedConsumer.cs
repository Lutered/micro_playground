﻿using MassTransit;
using MediatR;
using Shared.Contracts;
using UsersAPI.DTOs;
using UsersAPI.Infrastructure.Commands;

namespace UsersAPI.Consumers
{
    public class UserCreatedConsumer(
        IMediator _mediator)
      : IConsumer<UserCreated>
    {
        public async Task Consume(ConsumeContext<UserCreated> context)
        {
            var contract = context.Message;

            await _mediator.Send(new CreateUserCommand(
                new AppUserDTO() 
                { 
                    UserName = contract.Username,
                    Email = contract.Email,
                    Age = contract.Age
                }
            ));
        }
    }
}
