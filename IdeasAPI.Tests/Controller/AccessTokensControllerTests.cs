using System.Threading.Tasks;
using IdeasAPI.Controllers;
using IdeasAPI.Domain;
using IdeasAPI.Models;
using IdeasAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using IdeasAPI.Tests.TestAsyncClasses;

namespace IdeasAPI.Tests.Controller
{
    public class AccessTokensControllerTests
    {
        private AccessTokensController _accessTokensController;

        private Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        public AccessTokensControllerTests()
        {
            var userData = new List<User>
            {
                new User{ email = "john@test.com", name = "John Doe", password = "154bfe72ea6550c26e5f88ce693d01b7", refresh_token = "test_refresh_token" },
            }.AsQueryable();

            var userMockSet = new Mock<DbSet<User>>();
            userMockSet.As<IAsyncEnumerable<User>>().Setup(m => m.GetEnumerator()).Returns(new TestAsyncEnumerator<User>(userData.GetEnumerator()));
            userMockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(userData.Provider));
            userMockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(userData.Expression);
            userMockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            userMockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());
            _userRepositoryMock.Setup(m => m.Query()).Returns(userMockSet.Object.AsQueryable());
            var mockCtx = new Mock<IdeasDbContext>();
            mockCtx.SetupGet(c => c.Users).Returns(userMockSet.Object);

            _configurationMock.Setup(c => c["Jwt:Key"]).Returns("veryVerySecretTestKey");
            _configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("http://52.202.84.134:5000/");
            _accessTokensController = new AccessTokensController(_configurationMock.Object, _userRepositoryMock.Object);
        }


        [Fact]
        public async Task Login_ShouldReturnAccessToken()
        {
            var login = new LoginModel
            {
                email = "john@test.com",
                password = "JoHn1003"
            };

            // Arrange

            // Act
            var result = await _accessTokensController.Post(login);

            // Assert
            Assert.NotNull(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }


        [Fact]
        public async Task Refresh_ShouldReturnAccessToken()
        {
            // Arrange

            // Act
            var result = await _accessTokensController.Refresh("test_refresh_token");

            // Assert
            Assert.NotNull(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }


        [Fact]
        public async Task Delete_ShouldReturnNoContent()
        {
            // Arrange

            // Act
            var result = await _accessTokensController.Delete("test_refresh_token");

            // Assert
            Assert.NotNull(result);

            var ncResult = result as NoContentResult;
            Assert.NotNull(ncResult);
            Assert.Equal(204, ncResult.StatusCode);
        }

    }
}
