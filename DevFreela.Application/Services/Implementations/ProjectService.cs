using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.DTOs;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services.Interfaces;

namespace DevFreela.Application.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IPaymentService _paymentService;

        public ProjectService(IProjectRepository projectRepository, IPaymentService paymentService)
        {
            _projectRepository = projectRepository;
            _paymentService = paymentService;
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

        public async Task<bool> Finish(FinishProjectInputModel inputModel)
        {
            var project = await _projectRepository.GetByIdAsync(inputModel.Id);

            // Chamada ao Microsserviço de Pagamentos SEM Mensageria
            //project?.Finish();

            var paymentInfoDto = new PaymentInfoDTO(inputModel.Id, inputModel.CreditCardNumber, inputModel.Cvv, inputModel.ExpiresAt, inputModel.FullName, project.TotalCost);

            // Chamada ao Microsserviço de Pagamentos SEM Mensageria
            //var result = await _paymentService.ProcessPayment(paymentInfoDto);
            //if (!result)
                //project.SetPaymentPending();

            // Chamada ao Microsserviço de Pagamentos COM Mensageria
            _paymentService.ProcessPayment(paymentInfoDto);
            project.SetPaymentPending();

            await _projectRepository.SaveChangesAsync();

            //return result;
            return true;
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