using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeasAPI.Controllers;
using IdeasAPI.Domain;
using IdeasAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using IdeasAPI.Tests.TestAsyncClasses;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace IdeasAPI.Tests.Controller
{

    public class CurrentUserControllerTests
    {
        private CurrentUserController _currentUserController;

        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        public CurrentUserControllerTests()
        {
            var userData = new List<User>
            {
                new User{ email = "john@test.com", name = "John Doe", password = "154bfe72ea6550c26e5f88ce693d01b7" },
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

            _currentUserController = new CurrentUserController(_userRepositoryMock.Object);
        }


        [Fact]
        public async Task Get_ShouldReturnUser()
        {
            // Arrange
            _currentUserController.ControllerContext = new ControllerContext();
            _currentUserController.ControllerContext.HttpContext = new DefaultHttpContext();
            _currentUserController.ControllerContext.HttpContext.User = new ClaimsPrincipal();
            _currentUserController.ControllerContext.HttpContext.User.AddIdentity(new ClaimsIdentity());
            ((ClaimsIdentity)_currentUserController.ControllerContext.HttpContext.User.Identity).AddClaim(new Claim(ClaimTypes.Name, "john@test.com"));

            // Act
            var result = await _currentUserController.Get();

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

    }
}
