using Xunit;
using Moq;
using AutoMapper;
using SkillSheetAPI.Services.Services;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Models.DTOs;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Configuration;


namespace SkillSheetAPI.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IAuthRepo> _mockAuthRepo;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly Mock<IAuthService> _mockAuthService;

        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockAuthRepo = new Mock<IAuthRepo>();
            _mockEmailService = new Mock<IEmailService>();
            _mockMapper = new Mock<IMapper>();
            _mockConfig = new Mock<IConfiguration>(); // Initialize _mockConfig
            _mockAuthService = new Mock<IAuthService>();

            _authService = new AuthService(_mockAuthRepo.Object, _mockMapper.Object, _mockEmailService.Object,_mockConfig.Object);
        }

        [Fact]
        public async Task GetAllUserService_ReturnsListOfUserDTOs()
        {
            // Arrange
            var userModels = new List<UserDTO> { new UserDTO { Username = "testuser" } };
            var userDTOs = new List<UserDTO> { new UserDTO { Username = "testuser" } };
            _mockAuthRepo.Setup(repo => repo.GetAllUser()).ReturnsAsync(userDTOs);
            _mockMapper.Setup(mapper => mapper.Map<List<UserDTO>>(userModels)).Returns(userDTOs);

            // Act
            var result =await _authService.GetAllUserService() ;

            // Assert
            result.Should().BeEquivalentTo(userDTOs);
        }

        [Fact]
        public void RegisterUserService_ReturnsUserRegisterDTO()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO { Username = "testuser", Password = "password", Email = "test@example.com" };
            var userDTO = new UserDTO {Userid=7, Username = "testuser", Email = "test@example.com" };
            _mockAuthRepo.Setup(repo => repo.RegisterUser(userRegisterDTO)).ReturnsAsync(userDTO);
            _mockMapper.Setup(mapper => mapper.Map<UserRegisterDTO>(userDTO)).Returns(userRegisterDTO);

            // Act
            var result = _authService.RegisterUserService(userRegisterDTO);

            // Assert
            result.Should().BeEquivalentTo(userDTO, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task LoginUserService_ReturnsUserModelAndToken()
        {
            // Arrange
            var userLoginDTO = new UserLoginDTO { Username = "testuser", Password = "password" };
            var userDTO = new UserDTO { Username = "testuser",Email="abc@gmail.com",Role="user",Userid=1 };
            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxIiwiRW1haWwiOiJhYmNAZ21haWwuY29tIiwiTmFtZSI6InRlc3R1c2VyIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoidXNlciIsIlJvbGUiOiJ1c2VyIiwiZXhwIjoxNzQzMTQyODM3fQ.Y1TZTgLe_c5Ey_VyfyY8h7bV0yrruJQItXgsixrnvAQ";
            _mockAuthRepo.Setup(repo => repo.LoginUser(userLoginDTO)).ReturnsAsync(userDTO); 
            _mockConfig.Setup(config => config[It.IsAny<string>()]).Returns("secret_key-secret-key-secret-key-secret-key");
            // Act
            var (user, generatedToken) =await _authService.LoginUserService(userLoginDTO);

            // Assert
            user.Should().Be(userDTO);
        }

        [Fact]
        public async Task UpdateUserService_ReturnsTrue_WhenUserIsUpdated()
        {
            // Arrange
            var userRegisterDTO = new UserRegisterDTO { Username = "testuser", Password = "password", Email = "test@example.com" };
            _mockAuthRepo.Setup(repo => repo.UpdateUser(userRegisterDTO)).ReturnsAsync(true);

            // Act
            var result =await _authService.UpdateUserService(userRegisterDTO);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserService_ReturnsTrue_WhenUserIsDeleted()
        {
            // Arrange
            var username = "testuser";
            var userDto = new UserDTO { Username = "testuser" };
            _mockAuthRepo.Setup(repo => repo.GetUserByUsername(username)).ReturnsAsync(userDto);
            _mockAuthRepo.Setup(repo => repo.DeleteUser(username)).ReturnsAsync(true);

            // Act
            var result =await  _authService.DeleteUserService(username);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserService_ReturnsFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "testuser";
            //_mockAuthRepo.Setup(repo => repo.GetUserByUsername(username)).ReturnsAsync((UserDTO)null);
            _mockAuthRepo.Setup(repo => repo.DeleteUser(username)).ReturnsAsync(false);


            // Act
            var result =await _authService.DeleteUserService(username)  ;

            // Assert
            result.Should().BeFalse();
        }
    }
}