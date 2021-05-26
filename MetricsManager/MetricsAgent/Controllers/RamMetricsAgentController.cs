using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using MetricsAgent.Responses;
using AutoMapper;

namespace MetricsAgent.Controllers
{
    [Route("api/metrics/ram")]
    [ApiController]
    public class RamMetricsAgentController : ControllerBase
    {
        private readonly ILogger<RamMetricsAgentController> _logger;
        private readonly IRamMetricsRepository _repository;
        private readonly IMapper _mapper;
        public RamMetricsAgentController(ILogger<RamMetricsAgentController> logger, IRamMetricsRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("byPeriod/from/{fromTime}/to/{toTime}")]
        public IActionResult GetByTimePeriod([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"Parameters passed to the method: fromTime = {fromTime}, toTime = {toTime}");
            IList<RamMetric> metrics = _repository.GetByTimePeriod(fromTime, toTime);
            var response = new AllRamMetricsResponse()
            {
                Metrics = new List<RamMetricDto>()
            };

            foreach (var metric in metrics)
            {
                response.Metrics.Add(_mapper.Map<RamMetricDto>(metric));
            }
            return Ok(response);
        }

        [HttpGet("from/{fromTime}/to/{toTime}")]
        public IActionResult GetMetricsFromAgent([FromRoute] DateTimeOffset fromTime, [FromRoute] DateTimeOffset toTime)
        {
            _logger.LogInformation($"fromTime = {fromTime}, toTime = {toTime}");
            return Ok();
        }
    }
}
