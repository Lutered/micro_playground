using Shared.Sagas;

namespace Shared.Sagas.CreateUser
{
    public class CreateUserSagaInstance : BaseSagaInstance
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
