using Microsoft.Extensions.Configuration;
using Playground_Tests.AuthAPI;

namespace Playground_Tests.Unit_Tests.AuthAPI.Mocks
{
    public static class ConfigurationMock
    {
        public static IConfiguration GetMock()
        {
            var configData = new Dictionary<string, string?> {
                {"Authorization:SecretKeyPath", Config.TokenPath}
            };

            IConfiguration configuration = new ConfigurationBuilder()
               .AddInMemoryCollection(configData)
               .Build();

            return configuration;
        }
    }
}
