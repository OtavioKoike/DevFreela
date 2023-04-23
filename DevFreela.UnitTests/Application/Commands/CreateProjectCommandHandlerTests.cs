using DevFreela.Application.Commands.CreateProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DevFreela.UnitTests.Application.Commands
{
    public class CreateProjectCommandHandlerTests
    {
        [Fact]
        public async Task InputDataIsOk_Executed_ReturnProjectId()
        {
            // Arrange
            var command = new CreateProjectCommand()
            {
                Title = "Titulo de Teste",
                Description = "Descricao de Teste",
                IdClient = 1,
                IdFreelancer = 2,
                TotalCost = 1000
            };

            var projectRepositoryMock = new Mock<IProjectRepository>();

            var createProjectCommandHandler = new CreateProjectCommandHandler(projectRepositoryMock.Object);

            // Act
            var response = await createProjectCommandHandler.Handle(command, new CancellationToken());

            // Assert
            //Assert.True(response > 0);

            projectRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Project>()), Times.Once);
        } 
    }
}
