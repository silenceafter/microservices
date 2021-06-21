using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MetricsManager.DAL.Interfaces;

namespace MetricsManager.Controllers
{
    [Route("api/agent")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        private readonly ILogger<AgentController> _logger;
        private readonly IAgentsRepository _repository;
        public AgentController(ILogger<AgentController> logger, IAgentsRepository repository)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog встроен в AgentController");
            _repository = repository;
        }

        [HttpGet("getList")]
        public IActionResult GetRegisteredMetrics()
        {
            _logger.LogInformation($"no parameters");
            var agentList = _repository.GetAgents();
            return Ok(agentList);
        }

        /// <summary>
        /// Добавить агента в список зарегистрированных агентов в системе. Указать AgentId агента, адрес подключения уникальный 
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///    /api/agent/register
        /// 
        /// <example>
        /// JSON in body:
        ///<code>
        ///     {
        ///         "AgentId": 1,
        ///         "AgentAddress": "http://localhost:5000"
        ///     }
        ///</code>
        /// </example>
        /// </remarks>
        /// <param name="agent"></param>
        /// 
        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {
            _logger.LogInformation($"Register Agent = {agentInfo.AgentId}, Address = {agentInfo.AgentAddress}");
            _repository.RegisterAgent(agentInfo);
            return Ok();
        }

        [HttpPost("remove")]
        public IActionResult RemoveAgent([FromBody] AgentInfo agentInfo)
        {
            _logger.LogInformation($"Remove AgentAddress = {agentInfo.AgentAddress}");
            _repository.RemoveAgent(agentInfo);
            return Ok();
        }

        [HttpPost("removeAll")]
        public IActionResult RemoveAllAgent([FromRoute] bool parameter)
        {
            _logger.LogInformation($"RemoveAll AgentAddress");
            _repository.RemoveAllAgent();
            return Ok();
        }
    }
}
