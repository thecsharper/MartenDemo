using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Testing;

using MartenDemo;

using FluentAssertions;

namespace MartinDemo.AcceptanceTests
{
    public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public void Test1()
        {

        }
    }
}