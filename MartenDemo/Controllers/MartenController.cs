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

        [HttpGet("single")]
        [ProducesResponseType(typeof(MartenData),200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<MartenData> Get([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);
            
            await session.SaveChangesAsync();

            var output = _martenQueries.QueryData(martenData.Id);

            _logger.LogInformation(output.Id.ToString());

            return output;
        }

        [HttpGet("many")]
        [ProducesResponseType(typeof(List<MartenData>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<List<MartenData>> GetData([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);

            await session.SaveChangesAsync();

            var outputList = new List<MartenData>();

            // TODO add a list method
            var output = _martenQueries.QueryData(martenData.Id);

            outputList.Add(output);

            _logger.LogInformation(output.Id.ToString());

            return outputList;
        }

        [HttpPost("stream")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public Task Stream([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);

            session.SaveChanges();

            var output = _martenQueries.QueryData(martenData.Id);

            _logger.LogInformation(output.Id.ToString());

            return session.Json.WriteById<MartenData>(output.Id, HttpContext);
        }
    }
}