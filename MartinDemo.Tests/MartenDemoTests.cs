using Marten;

using Moq;
using Moq.AutoMock;

using MartenDemo.Controllers;
using MartenDemo.Models;
using MartenDemo;

namespace MartinDemo.Tests
{
    public class MartenDemoTests
    {
        [Fact]
        public async Task Marten_Controller_Get()
        {
            var session = new Mock<IDocumentSession>();

            var martenInput = new MartenData
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow
            };

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueries>(mock => mock.QueryData(It.IsAny<Guid>()) == martenInput);
            var controller = mocker.CreateInstance<MartenController>();

            await controller.Get(session.Object, martenInput);
        }

        [Fact]
        public void Marten_Controller_Stream()
        {
            var session = new Mock<IDocumentSession>();

            var martenInput = new MartenData
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow
            };

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueries>(mock => mock.QueryData(It.IsAny<Guid>()) == martenInput);
            var controller = mocker.CreateInstance<MartenController>();
            
            controller.Stream(session.Object, martenInput);
        }
    }
}