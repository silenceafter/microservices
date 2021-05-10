﻿using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace MetricsAgentTests
{
    public class HddControllerAgentUnitTests
    {
        private HddMetricsAgentController controller;
        public HddControllerAgentUnitTests()
        {
            controller = new HddMetricsAgentController();
        }

        [Fact]
        public void GetMetricsFromAgent_Returns_Ok()
        {
            var fromTime = TimeSpan.FromSeconds(0);
            var toTime = TimeSpan.FromSeconds(100);

            //Act
            var result = controller.GetMetricsFromAgent(fromTime, toTime);

            //Assert
            _ = Assert.IsAssignableFrom<IActionResult>(result);
        }
    }
}
