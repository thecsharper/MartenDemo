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

        [HttpGet("create")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<bool> Create([FromServices] IDocumentSession session)
        {
            for (int i = 0; i < 1000; i++)
            {
                session.Store(new MartenData() { Date = DateTime.UtcNow, Id = Guid.NewGuid(), Text = $"Text string {i}" });
            }

            await session.SaveChangesAsync();

            _logger.LogInformation("Created test records");

            return true;
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(MartenData), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public List<MartenData> Search([FromServices] IDocumentSession session, [FromQuery] string input)
        {
            var output = _martenQueries.GetByString(input);

            _logger.LogInformation(input);

            return output;
        }

        [HttpGet("count")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public int Count([FromServices] IDocumentSession session)
        {
            var output = _martenQueries.GetCount();

            _logger.LogInformation(output.ToString());

            return output;
        }

        [HttpGet("single")]
        [ProducesResponseType(typeof(MartenData), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<MartenData> Get([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);

            await session.SaveChangesAsync();

            var output = _martenQueries.GetSingleItem(martenData.Id);

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

            // TODO add a list method
            var output = _martenQueries.GetManyItems(martenData.Id);

            output.ForEach(x => _logger.LogInformation(x.Id.ToString()));

            return output;
        }

        [HttpPost("stream")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public Task Stream([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);

            session.SaveChanges();

            var output = _martenQueries.GetSingleItem(martenData.Id);

            _logger.LogInformation(output.Id.ToString());

            return session.Json.WriteById<MartenData>(output.Id, HttpContext);
        }

        [HttpGet("event")]
        [ProducesResponseType(typeof(MartenData), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public string Event([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            var s = session.Events.StartStream<MartenData>(martenData);
            session.SaveChanges();

            martenData.Text = "Updated text";

            // Append more events to the same stream
            session.Events.Append(martenData.Id);
            session.SaveChanges();

            //TODO move to queries class to moq
            var stream = session.Events.FetchStream(s.Id);

            var streamId = stream.First().StreamId.ToString();

            _logger.LogInformation(streamId);

            return streamId;
        }
    }
}