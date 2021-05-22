using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MetricsManager.Controllers
{
    [Route("api/agent")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly ILogger<AgentController> _logger;
        public AgentController(ILogger<AgentController> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в AgentController");
        }

        [HttpGet("getList")]
        public IActionResult GetRegisteredMetrics()
        {
            _logger.LogInformation($"no parameters");
            return Ok();
        }

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {            
            return Ok();
        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"agentId = {agentId}");
            return Ok();
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            _logger.LogInformation($"agentId = {agentId}");
            return Ok();
        }      
    }
}
