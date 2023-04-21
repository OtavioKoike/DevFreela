using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
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
        //api/users/{id}/login
        [HttpPut("{id}/login")]
        public async Task<IActionResult> Login(int id, [FromBody] LoginInputModel inputModel)
        {
            var access = await _userService.Login(inputModel);

            if(!access)
                return BadRequest();

            return Ok();
        } 
    }
}