using Marten;

using FluentAssertions;
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


            var id = Guid.NewGuid();
            var martenDate = DateTime.UtcNow;

            var martenInput = new MartenData
            {
                Id = id,
                Date = martenDate
            };

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueries>(mock => mock.GetSingleItem(It.IsAny<Guid>()) == martenInput);
            var controller = mocker.CreateInstance<MartenController>();

            var result = await controller.Get(session.Object, martenInput);

            result.Id.Should().Be(id);
            result.Date.Should().Be(martenDate);
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
            mocker.Use<IMartenQueries>(mock => mock.GetSingleItem(It.IsAny<Guid>()) == martenInput);
            var controller = mocker.CreateInstance<MartenController>();
            
            controller.Stream(session.Object, martenInput);
        }
    }
}