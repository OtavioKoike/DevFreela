using DevFreela.Application.InputModels;
using DevFreela.Application.ViewModels;

namespace DevFreela.Application.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectViewModel>> GetAll();
        Task<ProjectDetailsViewModel> GetById(int id);
        Task<int> Create(NewProjectInputModel inputModel);
        Task Update(UpdateProjectInputModel inputModel);
        Task Delete(int id);
        Task CreateComment(CreateCommentInputModel inputModel);
        Task Start(int id);
        Task<bool> Finish(FinishProjectInputModel inputModel); 
    }
}