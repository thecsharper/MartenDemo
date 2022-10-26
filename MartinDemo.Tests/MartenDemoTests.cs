using Microsoft.Extensions.Logging;

using Marten;
using Marten.Linq;

using Moq;
using Moq.AutoMock;

using MartenDemo.Controllers;
using MartenDemo.Models;
using MartenDemo;

namespace MartinDemo.Tests
{
    public class MartenDemoTests
    {
        private readonly Mock<ILogger<MartenController>> _logger;
        private Mock<IMartenQueries> _martenQueries;

        public MartenDemoTests()
        {
            _logger = new Mock<ILogger<MartenController>>();
            _martenQueries = new Mock<IMartenQueries>();
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
            var mocker = new AutoMocker();
            var controller = mocker.CreateInstance<MartenController>();
            _martenQueries.Setup(x => x.QueryData(It.IsAny<IDocumentSession>(), It.IsAny<MartenData>())).Returns(martenInput);

            controller.Stream(session.Object, martenInput);
        }
    }
}