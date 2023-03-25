using DevFreela.Application.InputModels;
using DevFreela.Application.ViewModels;

namespace DevFreela.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetById(int id);
        Task<int> Create(CreateUserInputModel inputModel);
        Task<bool> Login(LoginInputModel inputModel);
    }
}