using AuthAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using MockQueryable;
using Moq;

namespace Playground_Tests.Unit_Tests.AuthAPI.Mocks
{
    public static class UserManagerMock
    {
        public static Mock<UserManager<AppUser>> GetMock()
        {
            var store = new Mock<IUserStore<AppUser>>();
            var mockUserManager = new Mock<UserManager<AppUser>>(
                store.Object, null, null, null, null, null, null, null, null
            );

            var existingUsers = new AppUser[]
            {
                new AppUser { UserName = "Existing_User" }
            };

            mockUserManager
               .Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
               .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(um => um.Users)
                .Returns(existingUsers.BuildMock());

            return mockUserManager;

            return mockUserManager;
        }
    }
}
