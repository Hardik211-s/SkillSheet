using Xunit;
using Moq;
using AutoMapper;
using SkillSheetAPI.Services.Services;
using SkillSheetAPI.Models.DTOs;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;

namespace SkillSheetAPI.Tests.Services
{
    public class UserSkillServiceTests
    {
        private readonly Mock<IUserSkillRepo> _mockUserSkillRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserSkillService _userSkillService;

        public UserSkillServiceTests()
        {
            _mockUserSkillRepo = new Mock<IUserSkillRepo>();
            _mockMapper = new Mock<IMapper>();
            _userSkillService = new UserSkillService(_mockUserSkillRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AllUserSkillService_ReturnsListOfUserAllDataDTOs()
        {
            // Arrange
            var userSkills = new List<UserAllDataDTO> { new UserAllDataDTO { Username = "testuser", Skill = "C#" } };
            _mockUserSkillRepo.Setup(repo => repo.AllUserSkill()).ReturnsAsync(userSkills);
            _mockMapper.Setup(mapper => mapper.Map<List<UserAllDataDTO>>(userSkills)).Returns(userSkills);

            // Act
            var result =await _userSkillService.AllUserSkillService();

            // Assert
            result.Should().BeEquivalentTo(userSkills);
        }

        [Fact]
        public async Task GetUserSkillService_ReturnsListOfUserAllDataDTOs()
        {
            // Arrange
            var userId = 1;
            var userSkills = new List<UserAllDataDTO> { new UserAllDataDTO { Username = "testuser", Skill = "C#" } };
            _mockUserSkillRepo.Setup(repo => repo.GetUserSkill(userId)).ReturnsAsync(userSkills);
            _mockMapper.Setup(mapper => mapper.Map<List<UserAllDataDTO>>(userSkills)).Returns(userSkills);

            // Act
            var result = await _userSkillService.GetUserSkillService(userId);

            // Assert
            result.Should().BeEquivalentTo(userSkills);
        }

        [Fact]
        public async Task AddUserSkillService_ReturnsDbUserSkillDTO()
        {
            // Arrange
            var userSkillDTO = new UserSkillDTO { UserId = 1, MyId = new int[] { 1, 2 }, ProficiencyLevel = "Expert", Experience = 5.0 };
            var dbUserSkillDTO = new DbUserSkillDTO { UserId = 1, SkillId = 1, UserskillId = 1, ProficiencyLevel = "Expert", Experience = 5 };
            _mockUserSkillRepo.Setup(repo => repo.AddUserSkill(userSkillDTO)).ReturnsAsync(dbUserSkillDTO);

            // Act
            var result = await _userSkillService.AddUserSkillService(userSkillDTO);

            // Assert
            result.Should().Be(dbUserSkillDTO);
        }

        [Fact]
        public async Task EditUserSkillService_ReturnsDbUserSkillDTO()
        {
            // Arrange
            var userSkillDTO = new DbUserSkillDTO { UserId = 1, SkillId = 1, UserskillId = 1, ProficiencyLevel = "Expert", Experience = 5 };
            _mockUserSkillRepo.Setup(repo => repo.EditUserSkill(userSkillDTO)).ReturnsAsync(userSkillDTO);
            _mockMapper.Setup(mapper => mapper.Map<DbUserSkillDTO>(userSkillDTO)).Returns(userSkillDTO);

            // Act
            var result = await _userSkillService.EditUserSkillService(userSkillDTO);

            // Assert
            result.Should().Be(userSkillDTO);
        }

        [Fact]
        public async Task DeleteUserSkillService_ReturnsDbUserSkillDTO()
        {
            // Arrange
            var userSkillId = 1;
            var dbUserSkillDTO = new DbUserSkillDTO { UserId = 1, SkillId = 1, UserskillId = 1, ProficiencyLevel = "Expert", Experience = 5 };
            _mockUserSkillRepo.Setup(repo => repo.DeleteUserSkill(userSkillId)).ReturnsAsync(dbUserSkillDTO);
            _mockMapper.Setup(mapper => mapper.Map<DbUserSkillDTO>(dbUserSkillDTO)).Returns(dbUserSkillDTO);

            // Act
            var result = await _userSkillService.DeleteUserSkillService(userSkillId);

            // Assert
            result.Should().Be(dbUserSkillDTO);
        }
    }
}