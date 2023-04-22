using DevFreela.Application.ViewModels;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services.Interfaces;
using MediatR;

namespace DevFreela.Application.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserViewModel>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        public LoginUserCommandHandler(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }
        public async Task<LoginUserViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            // Usar o mesmo algoritimo para criar o Hash da senha
            var passwordHash = _authService.ComputeSha256Hash(request.Password);

            //Buscar na base um user que tenha o e-mail e a senha em formato hash
            var user = await _userRepository.GetUserByEmailAndPasswordAsync(request.Email, passwordHash);

            if (user == null)
                return null;

            // Geração do Token com o user obtido na base
            var token = _authService.GenerateJwtToken(user.Email, user.Role);
            return new LoginUserViewModel(request.Email, token);
        }
    }
}
