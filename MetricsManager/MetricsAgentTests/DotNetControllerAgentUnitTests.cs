using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MetricsAgent.Controllers;
using MetricsAgent.DAL;
using MetricsAgent.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MetricsAgentTests
{
    public class DotNetControllerAgentUnitTests
    {
        private DotNetMetricsAgentController _controller;
        private Mock<IDotNetMetricsRepository> _mock;
        private Mock<ILogger<DotNetMetricsAgentController>> _logger;
        private readonly IMapper _mapper;

        public DotNetControllerAgentUnitTests()
        {
            _mock = new Mock<IDotNetMetricsRepository>();
            _logger = new Mock<ILogger<DotNetMetricsAgentController>>();
            _controller = new DotNetMetricsAgentController(_logger.Object, _mock.Object, _mapper);
        }

        [Fact]
        public void GetByTimePeriod_ReturnsOk()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(10);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(20);
            _mock.Setup(a => a.GetByTimePeriod(DateTimeOffset.FromUnixTimeSeconds(10), DateTimeOffset.FromUnixTimeSeconds(20))).Returns(new List<DotNetMetric>()).Verifiable();

            //Act
            var result = _controller.GetByTimePeriod(fromTime, toTime);

            //Assert
            _mock.Verify(r => r.GetByTimePeriod(DateTimeOffset.FromUnixTimeSeconds(10), DateTimeOffset.FromUnixTimeSeconds(20)), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
