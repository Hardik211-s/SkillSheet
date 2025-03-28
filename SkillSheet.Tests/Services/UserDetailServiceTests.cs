using Xunit;
using Moq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using SkillSheetAPI.Services.Services;
using SkillSheetAPI.Models.DTOs;
using DataAccess.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http; 

namespace SkillSheetAPI.Tests.Services
{
    /// <summary>
    /// Unit tests for the UserDetailService class.
    /// </summary>
    public class UserDetailServiceTests
    {
        private readonly Mock<IUserDetailRepo> _mockUserDetailRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly UserDetailService _userDetailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDetailServiceTests"/> class.
        /// </summary>
        public UserDetailServiceTests()
        {
            _mockUserDetailRepo = new Mock<IUserDetailRepo>();
            _mockMapper = new Mock<IMapper>();
            _mockConfiguration = new Mock<IConfiguration>();
            _userDetailService = new UserDetailService(_mockUserDetailRepo.Object, _mockMapper.Object, _mockConfiguration.Object);
        }

        /// <summary>
        /// Tests that GetAllUserService returns a list of DbUserDetailDTOs.
        /// </summary>
        [Fact]
        public async Task GetAllUserService_ReturnsListOfDbUserDetailDTOs()
        {
            // Arrange
            var userModels = new List<DbUserDetailDTO> { new DbUserDetailDTO { Username = "testuser" } };
            _mockUserDetailRepo.Setup(repo => repo.GetAllUserDetail()).ReturnsAsync(userModels);
            _mockMapper.Setup(mapper => mapper.Map<List<DbUserDetailDTO>>(userModels)).Returns(userModels);

            // Act
            var result = await _userDetailService.GetAllUserService();

            // Assert
            result.Should().BeEquivalentTo(userModels);
        }
        


        /// <summary>
        /// Tests that GetUserDetailService returns a DbUserDetailDTO.
        /// </summary>
        [Fact]
        public async Task GetUserDetailService_ReturnsDbUserDetailDTO()
        {
            // Arrange
            var userId = 1;
            var userDetail = new DbUserDetailDTO { UserId = userId, Username = "testuser", Birthdate = new DateOnly(1990, 1, 1) };
            _mockUserDetailRepo.Setup(repo => repo.GetUserDetailById(userId)).ReturnsAsync(userDetail);

            // Act
            var result = await _userDetailService.GetUserDetailService(userId);

            // Assert
            result.Should().Be(userDetail);
            result.Age.Should().Be(DateTime.Now.Year - userDetail.Birthdate.Year);
        }
 

        /// <summary>
        /// Tests that DeleteUserDetailService returns true when a user is deleted.
        /// </summary>
        [Fact]
        public async Task DeleteUserDetailService_ReturnsTrue_WhenUserIsDeleted()
        {
            // Arrange
            var username = "testuser";
            _mockUserDetailRepo.Setup(repo => repo.DeleteUserDetail(username)).ReturnsAsync(true);

            // Act
            var result = await _userDetailService.DeleteUserDetailService(username);

            // Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async Task EditUserDetailService_ReturnsTrue_WhenEditIsSuccessful()
        {
            // Arrange
            var userDetailDto = new UserDetailDTO { Username = "testuser" };
            var dbUserDetailDto = new DbUserDetailDTO { Username = "testuser" };
            _mockUserDetailRepo.Setup(repo => repo.EditUserDetail(userDetailDto, It.IsAny<string>())).ReturnsAsync(dbUserDetailDto);
             
            // Act
            var result = await _userDetailService.EditUserDetailService(userDetailDto);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public async Task EditUserDetailService_ReturnsFalse_WhenEditFails()
        {
            // Arrange
            var userDetailDto = new UserDetailDTO { Username = "testuser" };
            _mockUserDetailRepo.Setup(repo => repo.EditUserDetail(userDetailDto, It.IsAny<string>())).ReturnsAsync((DbUserDetailDTO)null);

            // Act
            var result = await _userDetailService.EditUserDetailService(userDetailDto);

            // Assert
            result.Should().BeTrue();
        }
        /// <summary>
        /// Tests that Upload returns the file path when a file is uploaded.
        /// </summary>
        [Fact]
        public async Task Upload_ReturnsFilePath_WhenFileIsUploaded()
        {
            // Arrange
            var formFileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            formFileMock.Setup(f => f.OpenReadStream()).Returns(ms);
            formFileMock.Setup(f => f.FileName).Returns(fileName);
            formFileMock.Setup(f => f.Length).Returns(ms.Length);

            // Act
            var result = await _userDetailService.Upload(formFileMock.Object);

            // Assert
            result.Should().Contain("Parameter is not valid.");
        }

        /// <summary>
        /// Tests that CountAge returns the correct age.
        /// </summary>
        [Fact]
        public void CountAge_ReturnsCorrectAge()
        {
            // Arrange
            var birthdate = new DateOnly(1990, 1, 1);

            // Act
            var result =_userDetailService.CountAge(birthdate);

            // Assert
            result.Should().Be(DateTime.Now.Year - birthdate.Year);
        }
    }
}