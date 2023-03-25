using DevFreela.Application.Queries.GetAllSkills;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/skills")]
    public class SkillsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public SkillsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Usando Services
            //var skills = await _skillService.GetAll();

            // Usando CQRS
            var query = new GetAllSkillsQuery();
            var skills = await _mediator.Send(query);
            
            return Ok(skills);
        }
    }
}
