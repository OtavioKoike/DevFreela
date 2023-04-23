using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevFreela.UnitTests.Application.Queries
{
    public class GetAllPojectsQueryHandlerTests
    {
        [Fact]
        public async Task ThreeProjectsExist_Executed_ReturnThreeProjectsViewModels()
        {
            // Arrange
            var query = new GetAllPojectsQuery("");

            var projects = new List<Project>()
            {
                new Project("Titulo de Teste 1", "Descricao de Teste 1", 1, 2, 1000),
                new Project("Titulo de Teste 2", "Descricao de Teste 2", 3, 4, 2000),
                new Project("Titulo de Teste 2", "Descricao de Teste 3", 5, 6, 3000),
            };

            var projectRepositoryMock = new Mock<IProjectRepository>();
            projectRepositoryMock.Setup(pr => pr.GetAllAsync().Result).Returns(projects);

            var getAllProjectsQueryHandler = new GetAllPojectsQueryHandler(projectRepositoryMock.Object);

            // Act
            var response = await getAllProjectsQueryHandler.Handle(query, new CancellationToken());

            // Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response);
            Assert.Equal(projects.Count, response.Count);

            projectRepositoryMock.Verify(pr => pr.GetAllAsync().Result, Times.Once);

        }
    }
}
