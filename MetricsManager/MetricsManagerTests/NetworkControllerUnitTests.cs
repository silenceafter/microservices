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
    public class NetworkControllerUnitTests
    {       
        private NetworkMetricsController _controller;
        private Mock<ILogger<NetworkMetricsController>> _logger;
        private Mock<INetworkMetricsRepository> _mock;
        private readonly IMapper _mapper;

        public NetworkControllerUnitTests()
        {
            _mock = new Mock<INetworkMetricsRepository>();
            _logger = new Mock<ILogger<NetworkMetricsController>>();
            _controller = new NetworkMetricsController(_logger.Object, _mock.Object, _mapper);
        }

        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var fromTime = DateTimeOffset.FromUnixTimeSeconds(20);
            var toTime = DateTimeOffset.FromUnixTimeSeconds(200);
            _mock.Setup(a => a.GetMetricsFromAgent(agentId, fromTime, toTime)).Returns(new List<NetworkMetric>()).Verifiable();

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
            _mock.Setup(a => a.GetMetricsFromAllCluster(fromTime, toTime)).Returns(new List<NetworkMetric>()).Verifiable();

            //Act
            var result = _controller.GetMetricsFromAllCluster(fromTime, toTime);

            //Assert
            _mock.Verify(r => r.GetMetricsFromAgent(1, DateTimeOffset.FromUnixTimeSeconds(20), DateTimeOffset.FromUnixTimeSeconds(200)), Times.AtMostOnce());
            _logger.Verify();
        }
    }
}
