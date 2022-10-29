using Marten;

using FluentAssertions;
using Moq;
using Moq.AutoMock;

using MartenDemo.Controllers;
using MartenDemo.Models;
using MartenDemo;
using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace MartinDemo.Tests
{
    [Category("Marten Tests")]
    public class MartenDemoTests
    {
        private readonly Mock<ILogger<MartenController>> _logger;

        public MartenDemoTests()
        {
            _logger = new Mock<ILogger<MartenController>>();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Marten_Controller_Get()
        {
            var session = new Mock<IDocumentSession>();

            var martenInput = GetMartenData();

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueries>(mock => mock.GetSingleItem(It.IsAny<Guid>()) == martenInput);
            var controller = mocker.CreateInstance<MartenController>();

            var result = await controller.Get(session.Object, martenInput);

            result.Id.Should().Be(martenInput.Id);
            result.Date.Should().Be(martenInput.Date);
            result.Text.Should().Be(martenInput.Text);

            VerifyLogging();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Marten_Controller_GetData()
        {
            var session = new Mock<IDocumentSession>();

            var martenList = new List<MartenData>();

            var martenInput = GetMartenData();

            martenList.Add(martenInput);

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueries>(mock => mock.GetManyItems(It.IsAny<Guid>()) == martenList);
            var controller = mocker.CreateInstance<MartenController>();

            var result = await controller.GetData(session.Object, martenInput);

            result.First().Id.Should().Be(martenInput.Id);
            result.First().Date.Should().Be(martenInput.Date);
            result.First().Text.Should().Be(martenInput.Text);
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Marten_Controller_Stream()
        {
            var session = new Mock<IDocumentSession>();

            var martenInput = GetMartenData();

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueries>(mock => mock.GetSingleItem(It.IsAny<Guid>()) == martenInput);
            var controller = mocker.CreateInstance<MartenController>();
            
            controller.Stream(session.Object, martenInput);
        }

        private static MartenData GetMartenData()
        {
            return new MartenData
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Text = "Test Text"
            };
        }

        private void VerifyLogging()
        {
            _logger.Verify(x => x.Log(It.IsAny<LogLevel>(),
                           It.IsAny<EventId>(),
                           It.IsAny<It.IsAnyType>(),
                           It.IsAny<Exception>(),
                           (Func<It.IsAnyType, Exception, string>)
                           It.IsAny<object>()),
                           Times.Once);
        }
    }
}