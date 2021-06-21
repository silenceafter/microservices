using AutoMapper;
using MetricsManager.Controllers;
using MetricsManager.DAL.Interfaces;
using MetricsManager.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class RamControllerUnitTests
    {
        private RamMetricsController _controller;
        private Mock<ILogger<RamMetricsController>> _logger;
        private Mock<IRamMetricsRepository> _mock;
        private readonly IMapper _mapper;

        public RamControllerUnitTests()
        {
            _mock = new Mock<IRamMetricsRepository>();
            _logger = new Mock<ILogger<RamMetricsController>>();
            _controller = new RamMetricsController(_logger.Object, _mock.Object, _mapper);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(20);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(200);
            _mock.Setup(a => a.GetMetricsFromAgent(agentId, fromTime, toTime)).Returns(new List<RamMetric>()).Verifiable();

            //Act
            var result = _controller.GetMetricsFromAgent(agentId, fromTime, toTime);

            //Assert
            _mock.Verify(r => r.GetMetricsFromAgent(1, DateTimeOffset.FromUnixTimeSeconds(20), DateTimeOffset.FromUnixTimeSeconds(200)), Times.AtMostOnce());
            _logger.Verify();
        }

        [Fact]
        public void GetMetricsFromAllCluster_ReturnsOk()
        {
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(20);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(200);
            _mock.Setup(a => a.GetMetricsFromAllCluster(fromTime, toTime)).Returns(new List<RamMetric>()).Verifiable();

            //Act
            var result = _controller.GetMetricsFromAllCluster(fromTime, toTime);

            //Assert
            _mock.Verify(r => r.GetMetricsFromAgent(1, DateTimeOffset.FromUnixTimeSeconds(20), DateTimeOffset.FromUnixTimeSeconds(200)), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
