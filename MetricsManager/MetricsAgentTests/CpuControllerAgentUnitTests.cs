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
    public class CpuControllerAgentUnitTests
    {
        private CpuMetricsAgentController _controller;
        private Mock<ICpuMetricsRepository> _mock;
        private Mock<ILogger<CpuMetricsAgentController>> _logger;
        private readonly IMapper _mapper;

        public CpuControllerAgentUnitTests()
        {
            _mock = new Mock<ICpuMetricsRepository>();
            _logger = new Mock<ILogger<CpuMetricsAgentController>>();
            _controller = new CpuMetricsAgentController(_logger.Object, _mock.Object, _mapper);
        }

        [Fact]
        public void GetByTimePeriod_ReturnsOk()
        {
            //Arrange
            DateTimeOffset fromTime = DateTimeOffset.FromUnixTimeSeconds(10);
            DateTimeOffset toTime = DateTimeOffset.FromUnixTimeSeconds(20);
            _mock.Setup(a => a.GetByTimePeriod(DateTimeOffset.FromUnixTimeSeconds(10), DateTimeOffset.FromUnixTimeSeconds(20))).Returns(new List<CpuMetric>()).Verifiable();

            //Act
            var result = _controller.GetByTimePeriod(fromTime, toTime);

            //Assert
            _mock.Verify(r => r.GetByTimePeriod(DateTimeOffset.FromUnixTimeSeconds(10), DateTimeOffset.FromUnixTimeSeconds(20)), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
