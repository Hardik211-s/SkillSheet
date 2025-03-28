using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Controllers;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Models.DTOs;
using FluentAssertions;

namespace SkillSheetAPI.Tests.Controllers

{
    public class UserSkillControllerTests
    {
        private readonly Mock<IUserSkillService> _mockUserSkillService;
        private readonly UserSkillController _controller;

        public UserSkillControllerTests()
        {
            _mockUserSkillService = new Mock<IUserSkillService>();
            _controller = new UserSkillController(_mockUserSkillService.Object);
        }

        [Fact]
        public async Task GetUserSkill_ReturnsOkResult_WithListOfUserSkills()
        {
            // Arrange
            var userSkillList = new List<UserAllDataDTO> { new UserAllDataDTO { Username = "Hardik", Skill = "C#" } };
            _mockUserSkillService.Setup(service => service.AllUserSkillService()).ReturnsAsync(userSkillList);

            // Act
            var result = await _controller.GetUserSkill() as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { userSkill = userSkillList });
        }

        [Fact]
        public async Task GetUserSkillById_ReturnsOkResult_WithUserSkill()
        {
            // Arrange
            var userId = 5;
            var userSkill = new List<UserAllDataDTO> { new UserAllDataDTO { Username = "Hardik", Skill = "C#" } };
            _mockUserSkillService.Setup(service => service.GetUserSkillService(userId)).ReturnsAsync(userSkill);

            // Act
            var result = await _controller.GetUserSkill(userId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { userSkill = userSkill });
        }

        [Fact]
        public async Task AddUserSkill_ReturnsOkResult_WithUserSkill()
        {
            // Arrange
            var userSkillDto = new UserSkillDTO { UserId = 5, MyId = new int[] { 1, 2 }, ProficiencyLevel = "Expert", Experience = 5.0 };
            var userSkill = new DbUserSkillDTO { UserId = 5, SkillId = 1, UserskillId = 1, ProficiencyLevel = "Expert", Experience = 5 };
            _mockUserSkillService.Setup(service => service.AddUserSkillService(userSkillDto)).ReturnsAsync(userSkill);

            // Act
            var result = await _controller.AddUserSkill(userSkillDto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = "User skill added!" });

        }

        [Fact]
        public async Task EditUserSkill_ReturnsOkResult_WithUserSkill()
        {
            // Arrange
            var userSkillDto = new DbUserSkillDTO { UserId = 5, SkillId = 1, UserskillId = 1, ProficiencyLevel = "Expert", Experience = 5 };
            _mockUserSkillService.Setup(service => service.EditUserSkillService(userSkillDto)).ReturnsAsync(userSkillDto);

            // Act
            var result = await _controller.EditUserSkill(userSkillDto) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = "User skill edited!" });
        }

        [Fact]
        public async Task DeleteUserSkill_ReturnsOkResult_WhenUserSkillIsDeleted()
        {
            // Arrange
            var userSkillId = 1;
            var userSkill = new DbUserSkillDTO { UserId = 5, SkillId = 1, UserskillId = 1, ProficiencyLevel = "Expert", Experience = 5 };
            _mockUserSkillService.Setup(service => service.DeleteUserSkillService(userSkillId)).ReturnsAsync(userSkill);

            // Act
            var result = await _controller.DeleteUserSkill(userSkillId) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = "User skill deleted!" });
        }
    }
}