using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Controllers;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Models.DTOs; 
using FluentAssertions;
using SkillSheetAPI.Resources;
namespace  SkillSheetAPI.Tests.Controllers
{
    public class UserDetailControllerTests
    {
        private readonly Mock<IUserDetailService> _mockUserDetailService;
        private readonly UserDetailController _controller;

        public UserDetailControllerTests()
        {
            _mockUserDetailService = new Mock<IUserDetailService>();
            _controller = new UserDetailController(_mockUserDetailService.Object);
        }

        [Fact]
        public async Task GetAllUserDetail_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var userList = new List<DbUserDetailDTO> { new DbUserDetailDTO { UserId = 5, Username = "Hardik" } };
            _mockUserDetailService.Setup(service => service.GetAllUserService()).ReturnsAsync(userList);
            //userList = JsonConvert.DeserializeObject(userList);
            // Act
            var result = await _controller.GetAllUserDetail() as OkObjectResult;

            
            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = GeneralResource.AllUserFound, allUserDetail = userList });
        }

        [Fact]
        public async Task GetAllUserDetail_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var userList = new List<DbUserDetailDTO>();
            _mockUserDetailService.Setup(service => service.GetAllUserService()).ReturnsAsync(userList);

            // Act
            var result = await _controller.GetAllUserDetail() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = GeneralResource.AllUserFound, allUserDetail = userList });
        }
       
       

        [Fact]
        public async Task UserDetailById_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = 5;
            var userDetail =  new DbUserDetailDTO { UserId = 5, Username = "Hardik" };

            _mockUserDetailService.Setup(service => service.GetUserDetailService(userId)).ReturnsAsync(userDetail);

            // Act
            var result =await _controller.UserDetailById(userId) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = GeneralResource.AllUserFound,userDetail });
        }

       

        

        [Fact]
        public async Task EditUserDetail_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userDetailDto = new UserDetailDTO { Username = "testuser" };
            _mockUserDetailService.Setup(service => service.EditUserDetailService(userDetailDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.EditUserDetail(userDetailDto) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = GeneralResource.UserEdit});
        }
    }
}