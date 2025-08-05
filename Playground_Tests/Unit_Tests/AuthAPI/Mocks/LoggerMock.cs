using Microsoft.Extensions.Logging;
using Moq;

namespace Playground_Tests.Unit_Tests.AuthAPI.Mocks
{
    public static class LoggerMock
    {
        public static Mock<ILogger<T>> GetMock<T>()
        {
            var mockLogger = new Mock<ILogger<T>>();

            mockLogger
                .Setup(l => l.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()));

            mockLogger
                .Setup(l => l.LogError(It.IsAny<string>(), It.IsAny<object[]>()));

            return mockLogger;
        }
    }
}
