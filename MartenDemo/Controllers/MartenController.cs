using MartenDemo.Models;
using Microsoft.AspNetCore.Mvc;

using Marten;

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

        [HttpGet(Name = "GetData")]
        public MartenData Get([FromServices] IDocumentSession session, [FromQuery] MartenData martenData)
        {
            session.Store(martenData);
            session.SaveChangesAsync();
            return martenData;
        }
    }
}