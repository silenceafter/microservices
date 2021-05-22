using MetricsAgent;
using MetricsAgent.Controllers;
using Microsoft.AspNetCore.Mvc;
using MetricsAgent.DAL;
using MetricsAgent.Responses;
using MetricsAgent.Models;
using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using AutoMapper;

namespace MetricsAgentTests
{
    public class CpuControllerAgentUnitTests
    {        
        private CpuMetricsAgentController _controller;
        private Mock<ICpuMetricsRepository> _mock;
        private Mock<ILogger<CpuMetricsAgentController>> _logger;
        private IMapper _mapper;

        public CpuControllerAgentUnitTests()
        {
            _mock = new Mock<ICpuMetricsRepository>();
            _logger = new Mock<ILogger<CpuMetricsAgentController>>();           
            _controller = new CpuMetricsAgentController(_logger.Object, _mock.Object, _mapper);
        }

        [Fact]
        public void GetByTimePiriod_TestMethod()
        {
            //Arrange
            var valueList = new List<CpuMetric>();
            _mock.Setup(repository => repository.GetByTimePeriod(
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
                .Returns(valueList);
            
            //Act
            IActionResult result = _controller.GetByTimePeriod(
                DateTimeOffset.FromUnixTimeSeconds(100),
                DateTimeOffset.FromUnixTimeSeconds(200)); 
                        
            //Assert
            _mock.Verify(repository => 
                repository
                    .GetByTimePeriod(
                    DateTimeOffset.FromUnixTimeSeconds(100),
                    DateTimeOffset.FromUnixTimeSeconds(200)), 
                    Times.AtLeastOnce());
        }
    }
}
