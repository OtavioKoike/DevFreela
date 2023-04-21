using DevFreela.Application.InputModels;
using FluentValidation;
using System.Text.RegularExpressions;

namespace DevFreela.Application.Validators
{
    public class CreateUserInputModelValidator : AbstractValidator<CreateUserInputModel>
    {

        public CreateUserInputModelValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress()
                .WithMessage("E-mail não válido!"); // Definindo mensagem de retorno caso de erro

            RuleFor(u => u.Password)
                .Must(ValidPassword)
                .WithMessage("Senha deve conter pelo menos 8 caracteres, um numero, uma letra maiuscula, uma minuscula e um caractere especial.");

            RuleFor(u => u.FullName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Nome é obrigatorio!");
        }

        private bool ValidPassword(string password)
        {
            var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");

            return regex.IsMatch(password);
        }
    }
}
