//using AuthAPI.Sagas.Instances;
//using MassTransit;
//using Shared.Models.Contracts.User.PublishEvents;
//using Shared.Models.Contracts.User.Requests.CreateUser;
//using Shared.Models.Contracts.User.Requests.CreateUser.Events;

//namespace AuthAPI.Sagas
//{
//    public class CreateUserStateMachine : MassTransitStateMachine<UserSagaInstance>
//    {
//        public State Request { get; private set; }
//        public State Completed { get; private set; }
//        public State Failed { get; private set; }

//        public Event<CreateUserRequest> CreateEvent { get; private set; } = null!;
//        public Event<CreateUserRequestCompleted> CompletedEvent { get; private set; } = null!;

//        public CreateUserStateMachine()
//        {
//            InstanceState(x => x.CurrentState);

//            Event(() => CreateEvent, x => x.CorrelateById(c => c.Message.Id));

//            Initially(
//                When(CreateEvent)
//                    .Then(context =>
//                    {
//                        context.Saga.CreatedAt = DateTime.Now;
//                        context.Saga.UserName = context.Message.Username;
//                    })
//                    .TransitionTo(Request)
//                    .Finalize()
//            );

//            During(Request,
//                When(CompletedEvent)
//                    .Then(context =>
//                    {

//                    })
//                    .TransitionTo(Completed)
//                    .Finalize());

//            During(Request,
//               When(CompletedEvent)
//                   .Then(context =>
//                   {

//                   })
//                   .TransitionTo(Completed));


//            SetCompletedWhenFinalized();
//        }
//    }
//}
