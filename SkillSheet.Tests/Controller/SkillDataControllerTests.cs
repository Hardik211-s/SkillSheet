using Microsoft.AspNetCore.Mvc;
using Moq;
using SkillSheetAPI.Controllers;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces; 
using Xunit;

namespace SkillSheetAPI.Tests.Controllers
{
    public class SkillDataControllerTests
    {
        private readonly Mock<ISkillDataService> _mockSkillDataService;
        private readonly SkillDataController _controller;

        public SkillDataControllerTests()
        {
            _mockSkillDataService = new Mock<ISkillDataService>();
            _controller = new SkillDataController(_mockSkillDataService.Object);
        }

        [Fact]
        public async Task GetSkillCategory_ReturnsOkResult_WithCategories()
        {
            // Arrange
            var mockCategories = new List<SkillCategoryDTO> { new SkillCategoryDTO { SkillCategoryId= 1,SkillCategoryName= "Programming & Development",
      IconName= "fa fa-code text-primary"} };
            _mockSkillDataService.Setup(s => s.GetSkillCategoryService()).ReturnsAsync(mockCategories);

            // Act
            var result =await _controller.GetSkillCategory() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(mockCategories, result.Value.GetType().GetProperty("category")?.GetValue(result.Value));
        }

        [Fact]
        public async Task GetSkillSubcategory_ReturnsOkResult_WithSubcategories()
        {
            // Arrange
            var mockSubcategories = new List<SkillSubcategoryDTO> { new SkillSubcategoryDTO { SkillSubcategoryId=1,SkillCategoryId= 1,
      SkillSubcategoryName= "Programming & Development",
      IconName= "fa fa-code text-primary"} };
            _mockSkillDataService.Setup(s => s.GetSkillSubcategoryService()).ReturnsAsync(mockSubcategories);

            // Act
            var result = await _controller.GetSkillSubcategory() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(mockSubcategories, result.Value.GetType().GetProperty("subCategory")?.GetValue(result.Value));
        }

        [Fact]
        public async Task GetSkillSubcategory_WithId_ReturnsOkResult()
        {
            // Arrange
            var mockSubcategories = new List<SkillSubcategoryDTO> { new SkillSubcategoryDTO { SkillSubcategoryId=1,SkillCategoryId= 1,
      SkillSubcategoryName= "Programming & Development",
      IconName= "fa fa-code text-primary"} };
            _mockSkillDataService.Setup(s => s.GetSkillSubcategoryService(1)).ReturnsAsync(mockSubcategories);

            // Act
            var result = await _controller.GetSkillSubcategory(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(mockSubcategories, result.Value.GetType().GetProperty("subCategory")?.GetValue(result.Value));
        }

        [Fact]
        public async Task GetSkill_ReturnsOkResult_WithSkills()
        {
            // Arrange
            var mockSkills = new List<SkillDTO> { new SkillDTO {SkillId=100, SkillSubcategoryId=1,
      SkillName= "React js",
      IconName= "fa fa-code text-primary"} };
            _mockSkillDataService.Setup(s => s.GetSkillService()).ReturnsAsync(mockSkills);

            // Act
            var result = await _controller.GetSkill() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(mockSkills, result.Value.GetType().GetProperty("skills")?.GetValue(result.Value));
        }

        [Fact]
        public async Task GetSkill_WithId_ReturnsOkResult()
        {
            // Arrange
            var mockSkills = new List<SkillDTO> { new SkillDTO {SkillId=100, SkillSubcategoryId=1,
      SkillName= "C#",
      IconName= "devicon-csharp-plain text-primary"} };
            _mockSkillDataService.Setup(s => s.GetSkillService(1)).ReturnsAsync(mockSkills);

            // Act
            var result = await _controller.GetSkill(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(mockSkills, result.Value.GetType().GetProperty("skills")?.GetValue(result.Value));
        }

        [Fact]
        public async Task GetSkillCategory_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            _mockSkillDataService.Setup(s => s.GetSkillCategoryService()).Throws(new Exception("Database error"));

            // Act
            var result =await _controller.GetSkillCategory() as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Database error", result.Value);
        }
    }
}
