using Xunit;
using Moq;
using SkillSheetAPI.Services.Services;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Models.DTOs;
using FluentAssertions;

namespace SkillSheetAPI.Tests.Services
{
    public class DashboardServiceTests
    {
        private readonly Mock<IUserSkillService> _mockUserSkillService;
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IUserDetailService> _mockUserDetailService;
        private readonly DashboardService _dashboardService;

        public DashboardServiceTests()
        {
            _mockUserSkillService = new Mock<IUserSkillService>();
            _mockAuthService = new Mock<IAuthService>();
            _mockUserDetailService = new Mock<IUserDetailService>();
            _dashboardService = new DashboardService(_mockUserSkillService.Object, _mockAuthService.Object, _mockUserDetailService.Object);
        }

        [Fact]
        public async Task GetDashboardData_ReturnsDashboardDataDTO()
        {
            // Arrange
            var userSkills = new List<UserAllDataDTO>
            {
                new UserAllDataDTO { Username = "user1", Skill = "C#", Experience = 5 },
                new UserAllDataDTO { Username = "user2", Skill = "Java", Experience = 3 }
            };
            var users = new List<UserDTO> { new UserDTO { Username = "user1" }, new UserDTO { Username = "user2" } };
            var userDetails = new List<DbUserDetailDTO> { new DbUserDetailDTO { Username = "user1" }, new DbUserDetailDTO { Username = "user2" } };

            _mockAuthService.Setup(service => service.GetAllUserService()).ReturnsAsync(users);
            _mockUserSkillService.Setup(service => service.AllUserSkillService()).ReturnsAsync(userSkills);
            _mockUserDetailService.Setup(service => service.GetAllUserService()).ReturnsAsync(userDetails);

            // Act
            var result =await _dashboardService.GetDashboardData();

            // Assert
            result.TotalUser.Should().Be(users.Count);
            result.TotalSkill.Should().Be(2); // C# and Java
            result.UserAllData.Should().BeEquivalentTo(userSkills);
            result.AllUserDetail.Should().BeEquivalentTo(userDetails);
            result.ExperienceAVG.Should().Be(4); // Average of 5 and 3, truncated to 4
        }
    }
}