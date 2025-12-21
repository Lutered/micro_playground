using AuthAPI.Data.Entities;
using AuthAPI.Data.Repositories.Interfaces;
using Moq;

namespace Playground_Tests.Unit_Tests.AuthAPI.Mocks
{
    public static class AutoRepoMock
    {
        public static Mock<IAuthRepository> GetMock()
        {
            var mockAuthRepo = new Mock<IAuthRepository>();

            mockAuthRepo
                .Setup(r => r.GetRefereshToken("correct_token", new CancellationToken()))
                .ReturnsAsync(new RefreshToken() { 
                    User = new AppUser() 
                    { 
                        Id = Guid.NewGuid(), 
                        UserName = "Existing_User" 
                    },
                    Token = Guid.NewGuid().ToString(),
                    Expires = DateTime.Now.AddDays(5),
                    IsUsed = false,
                    IsRevoked = false

                });

            mockAuthRepo
               .Setup(r => r.GetRefereshToken("expired_token", new CancellationToken()))
               .ReturnsAsync(new RefreshToken()
               {
                   User = new AppUser()
                   {
                       Id = Guid.NewGuid(),
                       UserName = "Existing_User"
                   },
                   Token = Guid.NewGuid().ToString(),
                   Expires = DateTime.Now.AddDays(-1),
                   IsUsed = false,
                   IsRevoked = false

               });

            mockAuthRepo
              .Setup(r => r.AddRefreshToken(It.IsAny<RefreshToken>()));

            mockAuthRepo
                .Setup(r => r.SaveChangesAsync(new CancellationToken()));

            return mockAuthRepo;
        }
    }
}
