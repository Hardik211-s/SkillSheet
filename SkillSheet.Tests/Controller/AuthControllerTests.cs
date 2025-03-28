using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Models.DTOs;
using SkillSheetAPI.Services.Interfaces;
using FluentAssertions;
using SkillSheetAPI.Controllers;

namespace SkillSheetAPI.Tests.Controllers
{
    /// <summary>
    /// Unit tests for the AuthController class.
    /// </summary>
    public class UserControllerTests
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly AuthController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControllerTests"/> class.
        /// </summary>
        public UserControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>(); // Mock dependency
            _mockEmailService = new Mock<IEmailService>();
            _controller = new AuthController(_mockAuthService.Object);

        }

        /// <summary>
        /// Tests that GetAllUser returns Ok with users when users exist.
        /// </summary>

        [Fact]
        public async Task GetAllUser_UsersExist_ReturnsOkWithUsers()
        {
            // Arrange
            var users = new List<UserDTO>
        {
            new UserDTO { Username = "JohnDoe", Email = "john@example.com", Role = "Admin" },
            new UserDTO { Username = "JaneDoe", Email = "jane@example.com", Role = "User" }
        };
            _mockAuthService.Setup(s => s.GetAllUserService()).ReturnsAsync(users);

            // Act
            var result = await _controller.GetAllUser() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = "All user found successfully !", allUsers = users });
        }

        /// <summary>
        /// Tests that GetAllUser returns Ok with an empty list when no users exist.
        /// </summary>
        [Fact]
        public async Task GetAllUser_NoUsers_ReturnsOkWithEmptyList()
        {
            // Arrange
            _mockAuthService.Setup(s => s.GetAllUserService()).ReturnsAsync(new List<UserDTO>());

            // Act
            var result = await _controller.GetAllUser() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = "All user found successfully !", allUsers = new List<UserDTO>() });
        }

        /// <summary>
        /// Tests that Register returns Ok when a valid user is registered.
        /// </summary>
        [Fact]
        public async Task Register_ValidUser_ReturnsOk()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDTO { Username = "JohnDoe", Email = "john@example.com", Role = "Admin" };
            var userDto = new UserDTO { Username = "JohnDoe", Email = "john@example.com", Role = "Admin" };
            _mockAuthService.Setup(s => s.RegisterUserService(userRegisterDto)).ReturnsAsync(userDto);

            // Act
            var result = await _controller.Register(userRegisterDto) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = "User register successfully !", user = userDto });
        }

        /// <summary>
        /// Tests that Login returns Ok with a token when a valid user logs in.
        /// </summary>
        [Fact]
        public async Task Login_ValidUser_ReturnsOkWithToken()
        {
            // Arrange
            var userDto = new UserLoginDTO { Username = "JohnDoe", Role = "Admin" };
            var userData = new UserDTO { Username = "JohnDoe", Email = "john@example.com", Role = "Admin", Userid = 1 };
            string token = "fake-jwt-token";

            _mockAuthService.Setup(s => s.LoginUserService(userDto)).ReturnsAsync((userData, token));

            // Act
            var result = await _controller.Login(userDto) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = "User login successfully!", userData, token });
        }

        /// <summary>
        /// Tests that Login returns NotFound when the user is not found.
        /// </summary>
        [Fact]
        public async Task Login_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var userDto = new UserLoginDTO { Username = "UnknownUser", Role = "Admin" };
            _mockAuthService.Setup(s => s.LoginUserService(userDto)).ReturnsAsync((null, null));

            // Act
            var result = await _controller.Login(userDto) as NotFoundObjectResult;

            // Assert
            result.Should().BeNull();
            result?.StatusCode.Should().Be(404);
            result?.Value.Should().BeEquivalentTo(new { message = "User not found" });
        }

        /// <summary>
        /// Tests that UpdateUser returns Ok when the user exists.
        /// </summary>
        [Fact]
        public async Task UpdateUser_UserExists_ReturnsOk()
        {
            // Arrange
            var userDto = new UserRegisterDTO { Username = "JohnDoe", Email = "john@example.com", Role = "Admin" };
            _mockAuthService.Setup(s => s.UpdateUserService(userDto)).ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateUser(userDto) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = "User Updated successfully" });
        }

        /// <summary>
        /// Tests that UpdateUser returns NotFound when the user is not found.
        /// </summary>
        [Fact]
        public async Task UpdateUser_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            var userDto = new UserRegisterDTO { Username = "UnknownUser" };
            _mockAuthService.Setup(s => s.UpdateUserService(userDto)).ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateUser(userDto) as NotFoundObjectResult;

            // Assert
            result.Should().BeNull();
            result?.StatusCode.Should().Be(404);
            result?.Value.Should().BeEquivalentTo(new { message = "User not found" });
        }

        /// <summary>
        /// Tests that DeleteUser returns Ok when the user exists.
        /// </summary>

        [Fact]
        public async void DeleteUser_UserExists_ReturnsOk()
        {
            // Arrange
            string username = "JohnDoe";
            _mockAuthService.Setup(s => s.DeleteUserService(username)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteUser(username) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result?.StatusCode.Should().Be(200);
            result?.Value.Should().BeEquivalentTo(new { message = "User deleted successfully" });
        }

        /// <summary>
        /// Tests that DeleteUser returns NotFound when the user is not found.
        /// </summary>
        [Fact]
        public async Task DeleteUser_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            string username = "UnknownUser";
            _mockAuthService.Setup(s => s.DeleteUserService(username)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteUser(username) as NotFoundObjectResult;

            // Assert
            result.Should().BeNull();
            result?.StatusCode.Should().Be(404);
            result?.Value.Should().BeEquivalentTo(new { message = "User not found" });
        }


    }
}