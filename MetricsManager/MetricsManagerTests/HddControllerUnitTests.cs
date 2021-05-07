using MetricsManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsManagerTests
{
    public class HddControllerUnitTests
    {
        private HddMetricsController controller;
        public HddControllerUnitTests()
        {
            controller = new HddMetricsController();
        }
        [Fact]
        public void GetMetricsFromAgent_ReturnsOk()
        {
            var agentId = 1;
            var left = 2048;

            //Act
            var result = controller.GetMetricsFromAgent(agentId, left);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
