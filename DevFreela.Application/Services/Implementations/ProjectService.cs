using Dapper;
using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevFreela.Application.Services.Implementations
{
    public class ProjectService : IProjectService
    {

        private readonly DevFreelaDbContext _dbContext; 
        private readonly string _connectionString;
        public ProjectService(DevFreelaDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _connectionString = configuration.GetConnectionString("DevFreelaCs");
        }
        public int Create(NewProjectInputModel inputModel)
        {
            var project = new Project(inputModel.Title, inputModel.Description, inputModel.IdClient, inputModel.IdFreelancer, inputModel.TotalCost);
            _dbContext.Projects.Add(project);
            _dbContext.SaveChanges();
            
            return project.Id;
        }

        public void CreateComment(CreateCommentInputModel inputModel)
        {
            var comment = new ProjectComment(inputModel.Content, inputModel.IdProject, inputModel.IdUser);
            _dbContext.ProjectComments.Add(comment);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);

            project?.Cancel();
            _dbContext.SaveChanges();
        }

        public void Finish(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);

            project?.Finish();
            _dbContext.SaveChanges();
        }

        public List<ProjectViewModel> GetAll()
        {
            var projects = _dbContext.Projects;

            return projects
                .Select(p => new ProjectViewModel(p.Id, p.Title, p.CreateAt))
                .ToList();
            
        }

        public ProjectDetailsViewModel GetById(int id)
        {
            var project = _dbContext.Projects
                .Include(p => p.Client) //Incluir/Mapear o objeto do cliente
                .Include(p => p.Freelancer) //Incluir/Mapear o objeto do freelancer
                .Include(p => p.Comments) //Incluir/Mapear o objeto dos comentarios
                .SingleOrDefault(p => p.Id == id);

            if(project == null)
                return null;

            return new ProjectDetailsViewModel(
                project.Id, project.Title, project.Description, 
                project.TotalCost, project.StartedAt, project.FinishedAt,
                project.Client.FullName, project.Freelancer.FullName
            );
        }

        public void Start(int id)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == id);
            project?.Start();

            // Dapper
            using(var sqlConnection = new SqlConnection(_connectionString)){
                sqlConnection.Open();

                var script = "UPDATE Projects SET Status = @status, StartedAt = @startedAt WHERE Id = @id";
                sqlConnection.Execute(script, new { status = project.Status, startedAt = project.StartedAt, id });
            }
            
            // Entity Framework Core
            // _dbContext.SaveChanges();
        }

        public void Update(UpdateProjectInputModel inputModel)
        {
            var project = _dbContext.Projects.SingleOrDefault(p => p.Id == inputModel.Id);

            project.Update(inputModel.Title, inputModel.Description, inputModel.TotalCost);
            _dbContext.SaveChanges();
        }
    }
}