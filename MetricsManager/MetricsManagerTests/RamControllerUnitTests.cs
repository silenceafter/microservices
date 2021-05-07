using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class RamControllerUnitTests
    {
        private RamMetricsController controller;
        public RamControllerUnitTests()
        {
            controller = new RamMetricsController();
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var available = 1024;

            //Act
            var result = controller.GetMetricsFromAgent(agentId, available);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
