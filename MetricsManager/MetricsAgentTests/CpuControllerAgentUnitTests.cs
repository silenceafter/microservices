using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class CpuControllerAgentUnitTests
    {        
        private CpuMetricsAgentController controller;
        public CpuControllerAgentUnitTests()
        {
            controller = new CpuMetricsAgentController();
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
