using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;

namespace DevFreela.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> Create(CreateUserInputModel inputModel)
        {
            var user = new User(inputModel.FullName, inputModel.Email, inputModel.birthDate, inputModel.Password);
            await _userRepository.AddAsync(user);

            return user.Id;
        }

        public async Task<UserViewModel> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if(user == null)
                return null;
            
            return new UserViewModel(user.Id, user.FullName, user.Email, user.BirthDate);
        }

        public async Task<bool> Login(LoginInputModel inputModel)
        {
            var user = await _userRepository.GetByLoginAsync(inputModel.Email, inputModel.Password);
            
            if(user == null)
                return false;

            return true;
        }
    }
}