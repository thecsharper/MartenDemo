using Marten;

using FluentAssertions;
using Moq;
using Moq.AutoMock;

using MartenDemo.Controllers;
using MartenDemo.Models;
using MartenDemo;
using System.ComponentModel;

namespace MartinDemo.Tests
{
    [Category("Marten Tests")]
    public class MartenDemoTests
    {
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
                Date = DateTime.UtcNow
            };
        }
    }
}