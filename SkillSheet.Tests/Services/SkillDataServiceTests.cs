using Xunit;
using Moq;
using AutoMapper;
using SkillSheetAPI.Services.Services;
using SkillSheetAPI.Models.DTOs;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;

namespace SkillSheetAPI.Tests.Services
{
    public class SkillDataServiceTests
    {
        private readonly Mock<ISkillDataRepo> _mockSkillDataRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SkillDataService _skillDataService;

        public SkillDataServiceTests()
        {
            _mockSkillDataRepo = new Mock<ISkillDataRepo>();
            _mockMapper = new Mock<IMapper>();
            _skillDataService = new SkillDataService(_mockMapper.Object, _mockSkillDataRepo.Object);
        }

        [Fact]
        public async Task GetSkillCategoryService_ReturnsListOfSkillCategoryDTOs()
        {
            // Arrange
            var skillCategories = new List<SkillCategoryDTO> { new SkillCategoryDTO { SkillCategoryName = "Programming" } };
            _mockSkillDataRepo.Setup(repo => repo.GetSkillCategory()).ReturnsAsync(skillCategories);
            _mockMapper.Setup(mapper => mapper.Map<List<SkillCategoryDTO>>(skillCategories)).Returns(skillCategories);

            // Act
            var result =await _skillDataService.GetSkillCategoryService();

            // Assert
            result.Should().BeEquivalentTo(skillCategories);
        }

        [Fact]
        public async Task GetSkillSubcategoryService_ReturnsListOfSkillSubcategoryDTOs()
        {
            // Arrange
            var skillSubcategories = new List<SkillSubcategoryDTO> { new SkillSubcategoryDTO { SkillSubcategoryName = "Web Development" } };
            _mockSkillDataRepo.Setup(repo => repo.GetSkillSubcategory()).ReturnsAsync(skillSubcategories);
            _mockMapper.Setup(mapper => mapper.Map<List<SkillSubcategoryDTO>>(skillSubcategories)).Returns(skillSubcategories);

            // Act
            var result =await _skillDataService.GetSkillSubcategoryService();

            // Assert
            result.Should().BeEquivalentTo(skillSubcategories);
        }

        [Fact]
        public async Task GetSkillSubcategoryService_ByCategoryID_ReturnsListOfSkillSubcategoryDTOs()
        {
            // Arrange
            var categoryID = 1;
            var skillSubcategories = new List<SkillSubcategoryDTO> { new SkillSubcategoryDTO { SkillSubcategoryName = "Web Development" } };
            _mockSkillDataRepo.Setup(repo => repo.GetSkillSubcategory(categoryID)).ReturnsAsync(skillSubcategories);
            _mockMapper.Setup(mapper => mapper.Map<List<SkillSubcategoryDTO>>(skillSubcategories)).Returns(skillSubcategories);

            // Act
            var result =await _skillDataService.GetSkillSubcategoryService(categoryID);

            // Assert
            result.Should().BeEquivalentTo(skillSubcategories);
        }

        [Fact]
        public async Task GetSkillService_ReturnsListOfSkillDTOs()
        {
            // Arrange
            var skills = new List<SkillDTO> { new SkillDTO { SkillName = "C#" } };
            _mockSkillDataRepo.Setup(repo => repo.GetSkill()).ReturnsAsync(skills);
            _mockMapper.Setup(mapper => mapper.Map<List<SkillDTO>>(skills)).Returns(skills);

            // Act
            var result =await _skillDataService.GetSkillService();

            // Assert
            result.Should().BeEquivalentTo(skills);
        }

        [Fact]
        public async Task GetSkillService_BySubCategoryID_ReturnsListOfSkillDTOs()
        {
            // Arrange
            var subCategoryID = 1;
            var skills = new List<SkillDTO> { new SkillDTO { SkillName = "C#" } };
            _mockSkillDataRepo.Setup(repo => repo.GetSkill(subCategoryID)).ReturnsAsync(skills);
            _mockMapper.Setup(mapper => mapper.Map<List<SkillDTO>>(skills)).Returns(skills);

            // Act
            var result =await _skillDataService.GetSkillService(subCategoryID);

            // Assert
            result.Should().BeEquivalentTo(skills);
        }
    }
}