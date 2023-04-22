using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services.Interfaces;

namespace DevFreela.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public UserService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<int> Create(CreateUserInputModel inputModel)
        {
            // Criptografando o password
            var passwordHash = _authService.ComputeSha256Hash(inputModel.Password);
            var user = new User(inputModel.FullName, inputModel.Email, inputModel.birthDate, passwordHash, inputModel.Role);

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
            var user = await _userRepository.GetUserByEmailAndPasswordAsync(inputModel.Email, inputModel.Password);
            
            if(user == null)
                return false;

            return true;
        }
    }
}