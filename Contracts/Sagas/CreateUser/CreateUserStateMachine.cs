using MassTransit;
using Shared.Sagas.CreateUser.Events;

namespace Shared.Sagas.CreateUser
{
    public class CreateUserStateMachine : MassTransitStateMachine<CreateUserSagaInstance>
    {
        public State Pending { get; private set; }
        public State Completed { get; private set; }
        public State Failed { get; private set; }

        public Event<CreateUserEvent> CreateUser { get; private set; }
        public Event<CreateUserCompleteEvent> Complete { get; private set; }
        public Event<CreateUserFailureEvent> Failure { get; private set; }

        public CreateUserStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => CreateUser, x => x.CorrelateById(c => c.Message.RequestId));
            Event(() => Complete, x => x.CorrelateById(c => c.Message.RequestId));
            Event(() => Failure, x => x.CorrelateById(c => c.Message.RequestId));

            Initially(
                When(CreateUser)
                    .Then(context =>
                    {
                        context.Saga.CreatedAt = DateTime.UtcNow;
                        context.Saga.UserName = context.Message.Username;
                        context.Saga.Email = context.Message.Email;
                    })
                    .TransitionTo(Pending)
            );

            During(Pending,
                When(Complete)
                    .Then(context => 
                    {
                        context.Saga.ModifiedAt = DateTime.UtcNow;
                    })
                    .TransitionTo(Completed)
                    .Finalize());

            During(Pending,
               When(Failure)
                   .Then(context =>
                   {
                       context.Saga.ModifiedAt = DateTime.UtcNow;
                   })
                   .TransitionTo(Failed));

            SetCompletedWhenFinalized();
        }
    }
}
