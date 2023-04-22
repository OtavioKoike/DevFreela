using DevFreela.Application.Commands.LoginUser;
using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;
        public UsersController(IUserService userService, IMediator mediator)
        {
            _userService = userService;
            _mediator = mediator;
        }

        //api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //api/users
        [HttpPost]
        // Permite acesso na Action sem autenticacao
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] CreateUserInputModel inputModel)
        {
            // Movido para ValidatorFilters para não precisar repetir código
            //if (!ModelState.IsValid)
            //{
            //    var messages = ModelState
            //        .SelectMany(ms => ms.Value.Errors)
            //        .Select(e => e.ErrorMessage)
            //        .ToList();

            //    return BadRequest(messages);
            //}

            var id = await _userService.Create(inputModel);

            return CreatedAtAction(nameof(GetById), new { id = id }, inputModel);
        }

        //---------- Login ----------
        //api/users/login
        [HttpPut("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {

            // Usando Services
            //var access = await _userService.Login(inputModel);
            //if(!access)
            //    return BadRequest();

            // Usando CQRS
            var loginUserViewModel = await _mediator.Send(command);

            if(loginUserViewModel == null)
                return BadRequest();

            return Ok(loginUserViewModel);
        } 
    }
}