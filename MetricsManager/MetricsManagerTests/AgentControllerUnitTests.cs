using MetricsManager;
using MetricsManager.Controllers;
using MetricsManager.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MetricsManagerTests
{
    public class AgentControllerUnitTests
    {
        private AgentController _controller;
        private Mock<ILogger<AgentController>> _logger;
        private Mock<IAgentsRepository> _repository;

        public AgentControllerUnitTests()
        {
            _logger = new Mock<ILogger<AgentController>>();
            _repository = new Mock<IAgentsRepository>();
            _controller = new AgentController(_logger.Object, _repository.Object);
        }

        [Fact]
        public void GetRegisteredMetrics_ReturnsOk()
        {
            _repository.Setup(repo => repo.GetAgents()).Returns(new List<AgentInfo>()).Verifiable();
            var result = _controller.GetRegisteredMetrics();

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void RegisterAgent_ReturnsOk()
        {
            _repository.Setup(repo => repo.RegisterAgent(new AgentInfo())).Verifiable();
            var result = _controller.RegisterAgent(new AgentInfo());

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void RemoveAgent_ReturnsOk()
        {
            _repository.Setup(repo => repo.RemoveAgent(new AgentInfo())).Verifiable();
            var result = _controller.RemoveAgent(new AgentInfo());

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void RemoveAllAgent_ReturnsOk()
        {
            _repository.Setup(repo => repo.RemoveAllAgent()).Verifiable();
            var result = _controller.RemoveAllAgent(true);

            // Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
