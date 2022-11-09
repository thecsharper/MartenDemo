using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using MartenDemo;

using FluentAssertions;
using Microsoft.AspNetCore.Hosting;

namespace MartinDemo.AcceptanceTests
{
    // https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-7.0
    public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/search")]
        public async Task ProductSearchImage_valid_message_returns_global_event_id(string url)
        {
            var options = new WebApplicationFactoryClientOptions { AllowAutoRedirect = false };
            var client = _factory.CreateClient(options);

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
        }

    }
}