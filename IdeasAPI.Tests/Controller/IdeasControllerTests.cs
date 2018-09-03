using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeasAPI.Controllers;
using IdeasAPI.Domain;
using IdeasAPI.Models;
using IdeasAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using IdeasAPI.Tests.TestAsyncClasses;

namespace IdeasAPI.Tests.Controller
{
    
    public class IdeasControllerTests
    {
        private IdeasController _ideasController;

        private Mock<IIdeaRepository> _ideaRepositoryMock = new Mock<IIdeaRepository>();
        private Mock<IUserRepository> _userRepositoryMock = new Mock<IUserRepository>();

        public IdeasControllerTests()
        {
            var ideaData = new List<Idea>
            {
                new Idea{ id = "abcdef", content = "Idea 1", confidence = 5, ease = 6, impact = 7, created_at = System.DateTime.UtcNow },
                new Idea{ id = "ghijkl", content = "Idea 2", confidence = 8, ease = 9, impact = 10, created_at = System.DateTime.UtcNow },
            }.AsQueryable();

            var ideaMockSet = new Mock<DbSet<Idea>>();
            ideaMockSet.As<IAsyncEnumerable<Idea>>().Setup(m => m.GetEnumerator()).Returns(new TestAsyncEnumerator<Idea>(ideaData.GetEnumerator()));
            ideaMockSet.As<IQueryable<Idea>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Idea>(ideaData.Provider));
            ideaMockSet.As<IQueryable<Idea>>().Setup(m => m.Expression).Returns(ideaData.Expression);
            ideaMockSet.As<IQueryable<Idea>>().Setup(m => m.ElementType).Returns(ideaData.ElementType);
            ideaMockSet.As<IQueryable<Idea>>().Setup(m => m.GetEnumerator()).Returns(ideaData.GetEnumerator());
            _ideaRepositoryMock.Setup(m => m.Query()).Returns(ideaMockSet.Object.AsQueryable());
            var mockCtx = new Mock<IdeasDbContext>();
            mockCtx.SetupGet(c => c.Ideas).Returns(ideaMockSet.Object);

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
            mockCtx.SetupGet(c => c.Users).Returns(userMockSet.Object);

            _ideasController = new IdeasController(_ideaRepositoryMock.Object, _userRepositoryMock.Object);
        }


        [Fact]
        public async Task Get_ShouldReturnTwoIdeas()
        {
            // Arrange
            Functions.AddMockIdentityToContext(_ideasController, "john@test.com");

            // Act
            var result = await _ideasController.Get(1);

            // Assert
            Assert.NotNull(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
            var ideas = okResult.Value as IdeaListModel;
            Assert.Equal(2, ideas.Ideas.Count());
        }

        [Fact]
        public async Task Post_ShouldInsertIdea()
        {
            var idea = new IdeaModel
            {
                content = "My idea",
                impact = 6,
                ease = 7,
                confidence = 8
            };

            // Arrange
            Functions.AddMockIdentityToContext(_ideasController, "john@test.com");

            // Act
            var result = await _ideasController.Post(idea);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as CreatedResult;
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode);
        }

        [Fact]
        public async Task Put_ShouldUpdateIdea()
        {
            var idea = new IdeaModel
            {
                content = "Idea 2 edited",
                impact = 6,
                ease = 7,
                confidence = 8
            };

            // Arrange
            Functions.AddMockIdentityToContext(_ideasController, "john@test.com");

            // Act
            var result = await _ideasController.Put("ghijkl", idea);

            // Assert
            Assert.NotNull(result);

            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }


        [Fact]
        public async Task Delete_ShouldDeleteIdea()
        {
            // Arrange
            Functions.AddMockIdentityToContext(_ideasController, "john@test.com");

            // Act
            var result = await _ideasController.Delete("abcdef");

            // Assert
            Assert.NotNull(result);

            var noContentResult = result as NoContentResult;
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }
    }
}
