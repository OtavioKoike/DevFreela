using DevFreela.Application.InputModels;
using DevFreela.Application.ViewModels;

namespace DevFreela.Application.Services.Interfaces
{
    public interface IUserService
    {
        UserViewModel GetById(int id);
        int Create(CreateUserInputModel inputModel);
        bool Login(LoginInputModel inputModel);
    }
}