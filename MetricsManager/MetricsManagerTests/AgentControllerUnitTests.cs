using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;


namespace MetricsManagerTests
{
    public class AgentControllerUnitTests
    {
        private AgentController controller;
        public AgentControllerUnitTests()
        {
            controller = new AgentController();
        }
        [Fact]
        public void RegisterAgent_ReturnsOk()
        {
            var agentInfo = new MetricsManager.AgentInfo();

            //Act
            var result = controller.RegisterAgent(agentInfo);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void EnableAgentById_Returns_Ok()
        {
            var agentId = 1;

            //Act
            var result = controller.EnableAgentById(agentId);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }

        [Fact]
        public void DisableAgentById_Returns_Ok()
        {
            var agentId = 1;

            //Act
            var result = controller.DisableAgentById(agentId);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
