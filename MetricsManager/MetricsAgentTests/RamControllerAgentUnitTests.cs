using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class RamControllerAgentUnitTests
    {
        private RamMetricsAgentController controller;
        public RamControllerAgentUnitTests()
        {
            controller = new RamMetricsAgentController();
        }

        [Fact]
        public void GetAllMetrics_Returns_Ok()
        {
            //Act
            var result = controller.GetAllMetrics();

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
