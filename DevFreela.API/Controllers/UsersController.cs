using DevFreela.API.Example;
using DevFreela.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        public UsersController(ExampleClass example)
        {

        }

        //api/users/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok();
        }

        //api/users
        [HttpPost]
        public IActionResult Post([FromBody] CreateUserModel createUser)
        {
            return CreatedAtAction(nameof(Get), new { id = 1 }, createUser);
        }

        //---------- Login ----------
        //api/users/{id}/login
        [HttpPut("{id}/login")]
        public IActionResult Login(int id, [FromBody] LoginModel loginModel)
        {
            return NoContent();
        } 
    }
}