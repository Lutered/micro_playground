using AuthAPI.Settings;

namespace Playground_Tests.Unit_Tests.AuthAPI
{
    public static class AuthSettingsProvider
    {
        public static AuthSettings GetSettings()
        {
            return new AuthSettings()
            {
                SecretKeyPath = "../../../Files/private.key",
                AuthTokenExpireDays = "2",
                RefreshTokenExpireDays = "7",
                Issuer = "AuthAPI",
                Audience = "UserAPI"
            };
        }
    }
}
