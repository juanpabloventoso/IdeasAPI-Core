using System.Threading.Tasks;
using IdeasAPI.Controllers;
using IdeasAPI.Domain;
using IdeasAPI.Models;
using IdeasAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace IdeasAPI.Tests.Controller
{
    public class UsersControllerTests
    {
        private UsersController _usersController;

        private Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        public UsersControllerTests()
        {
            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("veryVerySecretTestKey");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("http://52.202.84.134:5000/");
            _usersController = new UsersController(_configurationMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Post_ShouldInsertUser()
        {
            var user = new UserModel
            {
                name = "John",
                email = "john@test.com",
                password = "JoHn1003"
            };

            // Arrange

            // Act
            var result = await _usersController.Post(user);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }
    }
}
