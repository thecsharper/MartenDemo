using Microsoft.Extensions.Logging;

using Marten;
using Marten.Linq;

using Moq;
using Moq.AutoMock;

using MartenDemo.Controllers;
using MartenDemo.Models;

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
        public void Marten_Controller_Stream()
        {
            var session = new Mock<IDocumentSession>();

            var martenInput = new MartenData()
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow
            };

            var testData = new[]
            {
               martenInput
            }.AsQueryable();

            var martenData = new Mock<IMartenQueryable<MartenData>>();

            session.Setup(x => x.Query<MartenData>()).Returns(martenData.Object);

            var mocker = new AutoMocker();
            var controller = mocker.CreateInstance<MartenController>();

            controller.Stream(session.Object, martenInput);
        }
    }
}