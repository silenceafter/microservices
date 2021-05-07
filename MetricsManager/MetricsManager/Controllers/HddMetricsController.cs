using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/metrics/hdd")]
    [ApiController]
    public class HddMetricsController : ControllerBase
    {
        [HttpGet("agent/{agentId}/left/{left}")]
        public IActionResult GetMetricsFromAgent([FromRoute] int agentId, [FromRoute] int left)
        {
            return Ok();
        }
    }
}
