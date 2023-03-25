using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;

namespace DevFreela.Application.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<int> Create(NewProjectInputModel inputModel)
        {
            var project = new Project(inputModel.Title, inputModel.Description, inputModel.IdClient, inputModel.IdFreelancer, inputModel.TotalCost);
            await _projectRepository.AddAsync(project);
            
            return project.Id;
        }

        public async Task CreateComment(CreateCommentInputModel inputModel)
        {
            var comment = new ProjectComment(inputModel.Content, inputModel.IdProject, inputModel.IdUser);
            await _projectRepository.AddCommentAsync(comment);
        }

        public async Task Delete(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            project?.Cancel();

            await _projectRepository.SaveChangesAsync();
        }

        public async Task Finish(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            project?.Finish();

            await _projectRepository.SaveChangesAsync();
        }

        public async Task<List<ProjectViewModel>> GetAll()
        {
            var projects =  await _projectRepository.GetAllAsync();

            return projects
                .Select(p => new ProjectViewModel(p.Id, p.Title, p.CreateAt))
                .ToList();
            
        }

        public async Task<ProjectDetailsViewModel> GetById(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);

            if (project == null)
                return null;

            return new ProjectDetailsViewModel(
                project.Id, project.Title, project.Description, 
                project.TotalCost, project.StartedAt, project.FinishedAt,
                project.Client.FullName, project.Freelancer.FullName
            );
        }

        public async Task Start(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            project?.Start();

            await _projectRepository.StartAsync(project);
        }

        public async Task Update(UpdateProjectInputModel inputModel)
        {
            var project = await _projectRepository.GetByIdAsync(inputModel.Id);

            project.Update(inputModel.Title, inputModel.Description, inputModel.TotalCost);
            await _projectRepository.SaveChangesAsync();
        }
    }
}