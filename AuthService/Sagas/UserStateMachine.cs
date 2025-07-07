using AuthAPI.Sagas.Instances;
using Contracts;
using MassTransit;

namespace AuthAPI.Sagas
{
    public class UserStateMachine : MassTransitStateMachine<UserState>
    {
        public State Created { get; private set; }
        public State Synchronized { get; private set; }

        public Event<UserCreated> UserCreated { get; private set; } = null!;

        public UserStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => UserCreated, x => x.CorrelateById(c => c.Message.Id));

            Initially(
                When(UserCreated)
                    .Then(context =>
                    {
                        context.Saga.CreatedAt = DateTime.Now;
                        context.Saga.UserName = context.Message.Username;
                    })
                    .TransitionTo(Created)
                    .Finalize()
            );

            //During(Created,
            //    When(OrderPaid)
            //        .Then(context => context.Instance.PaidAt = context.Data.Timestamp)
            //        .TransitionTo(Paid)
            //        .Finalize()
            //);

            SetCompletedWhenFinalized();
        }
    }
}
