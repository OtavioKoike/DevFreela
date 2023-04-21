using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DevFreela.API.Filters
{
    public class ValidatorFilter : IActionFilter
    {
        // Executado depois da Action
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        // Executado antes da Action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Validar se o objeto de entrada está correto
            if (!context.ModelState.IsValid)
            {
                // Em caso de erro, listar os erros e retornar
                var messages = context.ModelState
                    .SelectMany(ms => ms.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                context.Result = new BadRequestObjectResult(messages);
            }
        }
    }
}
