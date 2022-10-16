using MartenDemo.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IEnumerable<MartenData> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new MartenData
            {
                Date = DateTime.Now.AddDays(index),
                Summary = string.Empty
            })
            .ToArray();
        }
    }
}