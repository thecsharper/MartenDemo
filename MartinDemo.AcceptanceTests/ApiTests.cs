using System.Net;

using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

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
        [InlineData("/Marten/search?input=test&PageNumber=1&PageSize=10")]
        public async Task Search_api_returns_result(string url)
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