using Microsoft.Extensions.Logging;

using Marten;
using Moq;

using MartenDemo.Controllers;

namespace MartinDemo.Tests
{
    public class MartenDemoTests
    {
        private readonly Mock<ILogger<MartenController>> _logger;
        
        public MartenDemoTests()
        {
            _logger = new Mock<ILogger<MartenController>>();
        }

        [Fact]
        public void Test1()
        {
            var session = new Mock<IDocumentSession>();

            var controller = new MartenController(_logger.Object);
        }
    }
}