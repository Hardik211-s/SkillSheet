using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Controllers;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Models.DTOs;
using FluentAssertions;
using SkillSheetAPI.Resources;

namespace SkillSheetAPI.Tests.Controllers
{
    public class DashboardControllerTests
    {
        private readonly Mock<IDashboardService> _mockDashboardService;
        private readonly DashboardController _controller;

        public DashboardControllerTests()
        {
            _mockDashboardService = new Mock<IDashboardService>();
            _controller = new DashboardController(_mockDashboardService.Object);
        }

        [Fact]
        public async Task DashboardData_ReturnsOkResult_WithDashboardData()
        {
            // Arrange
            var dashboardData = new DashboardDataDTO
            {
                TotalUser = 10,
                TotalSkill = 5,
                UserAllData = new List<UserAllDataDTO>
                {
                    new UserAllDataDTO { Username = "Hardik", Skill = "C#" }
                },
                ExperienceAVG = 4.5,
                AllUserDetail = new List<DbUserDetailDTO>
                {
                    new DbUserDetailDTO { UserId = 1, Username = "Hardik" }
                }
            };
            _mockDashboardService.Setup(service => service.GetDashboardData()).ReturnsAsync(dashboardData);

            // Act
            var result =await _controller.DashboardData() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = GeneralResource.DashboardDataSuccess, data = dashboardData });
        }

        [Fact]
        public async Task DashboardData_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var exceptionMessage = "An error occurred";
            _mockDashboardService.Setup(service => service.GetDashboardData()).Throws(new System.Exception(exceptionMessage));

            // Act
            var result =await _controller.DashboardData() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(400);
            result?.Value.Should().Be(exceptionMessage);
        }
    }
}