using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsController : ControllerBase
    {
        [HttpGet("agent/{agentId}/available/{available}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] int available)
        {
            return Ok();
        }
    }
}
