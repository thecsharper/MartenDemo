using System.ComponentModel;
using Microsoft.Extensions.Logging;

using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Marten;

using MartenDemo.Controllers;
using MartenDemo.Models;
using MartenDemo;
using Marten.Events;

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
            mocker.Use<IMartenQueryBuilder>(mock => mock.GetSingleItem(It.IsAny<Guid>()) == martenInput);
            mocker.Use(_logger);
            var controller = mocker.CreateInstance<MartenController>();

            var result = await controller.Get(session.Object, martenInput);

            result.Id.Should().Be(martenInput.Id);
            result.Date.Should().Be(martenInput.Date);
            result.Text.Should().Be(martenInput.Text);

            VerifyLogging(Times.Once);

            var martenQueriesMock = mocker.GetMock<IMartenQueryBuilder>();
            martenQueriesMock.VerifyAll();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public async Task Marten_Controller_Create()
        {
            var session = new Mock<IDocumentSession>();

            var martenInput = GetMartenData();

            var mocker = new AutoMocker();
            mocker.Use(_logger);
            var controller = mocker.CreateInstance<MartenController>();

            var result = await controller.Create(session.Object);

            result.Should().Be(true);

            VerifyLogging(Times.Once);

            var martenQueriesMock = mocker.GetMock<IMartenQueryBuilder>();
            martenQueriesMock.VerifyAll();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Marten_Controller_Search()
        {
            var session = new Mock<IDocumentSession>();

            var martenList = new List<MartenData>();

            var martenInput = GetMartenData();

            martenList.Add(martenInput);

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueryBuilder>(mock => mock.GetByString(It.IsAny<string>()) == martenList);
            mocker.Use(_logger);
            var controller = mocker.CreateInstance<MartenController>();

            var result = controller.Search(session.Object, "Test Text");

            result.First().Id.Should().Be(martenInput.Id);
            result.First().Date.Should().Be(martenInput.Date);
            result.First().Text.Should().Be(martenInput.Text);

            VerifyLogging(Times.Once);

            var martenQueriesMock = mocker.GetMock<IMartenQueryBuilder>();
            martenQueriesMock.VerifyAll();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Marten_Controller_Count()
        {
            var session = new Mock<IDocumentSession>();

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueryBuilder>(mock => mock.GetCount() == 10);
            mocker.Use(_logger);
            var controller = mocker.CreateInstance<MartenController>();

            var result = controller.Count(session.Object);

            result.Should().Be(10);

            VerifyLogging(Times.Once);

            var martenQueriesMock = mocker.GetMock<IMartenQueryBuilder>();
            martenQueriesMock.VerifyAll();
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
            mocker.Use<IMartenQueryBuilder>(mock => mock.GetManyItems(It.IsAny<Guid>()) == martenList);
            mocker.Use(_logger);
            var controller = mocker.CreateInstance<MartenController>();

            var result = await controller.GetData(session.Object, martenInput);

            result.First().Id.Should().Be(martenInput.Id);
            result.First().Date.Should().Be(martenInput.Date);
            result.First().Text.Should().Be(martenInput.Text);

            VerifyLogging(Times.Once);

            var martenQueriesMock = mocker.GetMock<IMartenQueryBuilder>();
            martenQueriesMock.VerifyAll();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Marten_Controller_Stream()
        {
            var session = new Mock<IDocumentSession>();

            var martenInput = GetMartenData();

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueryBuilder>(mock => mock.GetSingleItem(It.IsAny<Guid>()) == martenInput);
            mocker.Use(_logger);
            var controller = mocker.CreateInstance<MartenController>();

            controller.Stream(session.Object, martenInput);

            VerifyLogging(Times.Once);

            var martenQueriesMock = mocker.GetMock<IMartenQueryBuilder>();
            martenQueriesMock.VerifyAll();
        }

        [Fact]
        [Trait("Category", "Unit")]
        public void Marten_Controller_Event()
        {
            var guid = Guid.NewGuid();

            var events = new Mock<IEvent>();
            events.Setup(x => x.Id).Returns(guid);

            var list = new List<IEvent>
            {
                events.Object
            };

            var session = new Mock<IDocumentSession>();
            session.Setup(x => x.Events.FetchStream(It.IsAny<Guid>(), 0, DateTime.Now, 0)).Returns(list);

            var martenInput = GetMartenData();

            var testString = "test string";

            var mocker = new AutoMocker();
            mocker.Use<IMartenQueryBuilder>(mock => mock.AddEvent(It.IsAny<MartenData>(), It.IsAny<string>()) == testString);
            mocker.Use(_logger);
            var controller = mocker.CreateInstance<MartenController>();

            var output = controller.Event(martenInput);

            output.Should().Be(testString);

            VerifyLogging(Times.Once);

            var martenQueriesMock = mocker.GetMock<IMartenQueryBuilder>();
            martenQueriesMock.VerifyAll();
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

        private void VerifyLogging(Func<Times> times)
        {
            _logger.Verify(
             x => x.Log(
                 It.Is<LogLevel>(l => l == LogLevel.Information),
                 It.IsAny<EventId>(),
                 It.Is<It.IsAnyType>((v, t) => true),
                 It.IsAny<Exception>(),
                 It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), times);
        }
    }
}