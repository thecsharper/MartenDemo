using Microsoft.AspNetCore.Mvc;

using Marten;
using Marten.AspNetCore;

using MartenDemo.Models;

namespace MartenDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MartenController : ControllerBase
    {
        private readonly ILogger<MartenController> _logger;

        public MartenController(ILogger<MartenController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<MartenData> Get([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);
            
            await session.SaveChangesAsync();

            var output = session.Query<MartenData>().First(x=> x.Id == martenData.Id);

            return output;
        }

        [HttpPost]
        public Task Stream([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);

            session.SaveChanges();

            var output = session.Query<MartenData>().First(x => x.Id == martenData.Id);

            return session.Json.WriteById<MartenData>(output.Id, HttpContext);
        }
    }
}