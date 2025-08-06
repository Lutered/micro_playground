using AuthAPI.Data.Entities;
using AuthAPI.Intrefaces;
using Moq;

namespace Playground_Tests.Unit_Tests.AuthAPI.Mocks
{
    public static class AutoRepoMock
    {
        public static Mock<IAuthRepository> GetMock()
        {
            var mockAuthRepo = new Mock<IAuthRepository>();

            mockAuthRepo
              .Setup(r => r.AddRefreshToken(It.IsAny<RefreshToken>()));

            mockAuthRepo
                .Setup(r => r.SaveChangesAsync());

            return mockAuthRepo;
        }
    }
}
