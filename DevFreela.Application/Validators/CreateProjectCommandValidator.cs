using DevFreela.Application.Commands.CreateProject;
using FluentValidation;

namespace DevFreela.Application.Validators
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(p => p.Title)
                .MaximumLength(50)
                .WithMessage("Tamanho maximo do Titulo é de 50 caracteres");

            RuleFor(p => p.Description)
                .MaximumLength(255)
                .WithMessage("Tamanho maximo da Descrição é de 255 caracteres");
        }
    }
}
