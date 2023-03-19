using DevFreela.Application.Queries.GetAllSkills;
using DevFreela.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/skills")]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillService _skillService;
        private readonly IMediator _mediator;
        
        public SkillsController(ISkillService skillService, IMediator mediator)
        {
            _skillService = skillService;
            _mediator = mediator;
        }

        [HttpGet]
        public async  Task<IActionResult> Get()
        {
            // Usando Services
            //var skills = _skillService.GetAll();

            // Usando CQRS
            var query = new GetAllSkillsQuery();
            var skills = await _mediator.Send(query);
            
            return Ok(skills);
        }
    }
}
