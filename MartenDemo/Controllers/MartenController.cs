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
        private readonly IMartenQueries _martenQueries;

        public MartenController(ILogger<MartenController> logger, IMartenQueries martenQueries)
        {
            _logger = logger;
            _martenQueries = martenQueries;
        }

        [HttpGet]
        public async Task<MartenData> Get([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);
            
            await session.SaveChangesAsync();

            var output = _martenQueries.QueryData(session, martenData);

            _logger.LogInformation(output.Id.ToString());

            return output;
        }

        [HttpPost]
        public Task Stream([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);

            session.SaveChanges();

            var output = _martenQueries.QueryData(session, martenData);

            _logger.LogInformation(output.Id.ToString());

            return session.Json.WriteById<MartenData>(output.Id, HttpContext);
        }
    }
}