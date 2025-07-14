using AuthAPI.Sagas.Instances;
using Contracts;
using MassTransit;

namespace AuthAPI.Sagas
{
    public class UserStateMachine : MassTransitStateMachine<UserSagaInstance>
    {
        public State Submit { get; private set; }
        public State Synchronized { get; private set; }
        public State Fault { get; private set; }

        public Event<UserCreated> SubmittedEvent { get; private set; } = null!;

        public UserStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => SubmittedEvent, x => x.CorrelateById(c => c.Message.Id));

            Initially(
                When(SubmittedEvent)
                    .Then(context =>
                    {
                        context.Saga.CreatedAt = DateTime.Now;
                        context.Saga.UserName = context.Message.Username;
                    })
                    .TransitionTo(Synchronized)
                    .Finalize()
            );

            SetCompletedWhenFinalized();
        }
    }
}
