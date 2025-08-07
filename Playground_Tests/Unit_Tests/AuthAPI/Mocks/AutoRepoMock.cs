﻿using AuthAPI.Data.Entities;
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
                .Setup(r => r.GetRefereshToken("correct_token"))
                .ReturnsAsync(new RefreshToken() { 
                    User = new AppUser() 
                    { 
                        Id = 1, 
                        UserName = "Existing_User" 
                    },
                    Token = Guid.NewGuid().ToString(),
                    Expires = DateTime.Now.AddDays(5),
                    IsUsed = false,
                    IsRevoked = false

                });

            mockAuthRepo
               .Setup(r => r.GetRefereshToken("expired_token"))
               .ReturnsAsync(new RefreshToken()
               {
                   User = new AppUser()
                   {
                       Id = 1,
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
                .Setup(r => r.SaveChangesAsync());

            return mockAuthRepo;
        }
    }
}
