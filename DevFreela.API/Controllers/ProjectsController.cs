using DevFreela.Application.Commands.CreateComment;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.InputModels;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/projects")]
    // Todos metodos precisam de uma autenticacao (Definida na program)
    // Porém é utilizado nesse ponto quando não está usando perfis (roles)
    //[Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMediator _mediator;
        
        public ProjectsController(IProjectService projectService, IMediator mediator)
        {
            _projectService = projectService;
            _mediator = mediator;
        }

        //api/projects?query=netcore
        [HttpGet] //return Ok sempre
        [Authorize(Roles = "client, freelancer" )] // Quando é utilizado roles, o Authorize fica no metodo especificando as roles que tem acesso
        public async Task<IActionResult> Get(string query)
        {
            // Usando Services
            //var projects = await _projectService.GetAll();

            var getAllProjectsQuery = new GetAllPojectsQuery(query);
            var projects = await _mediator.Send(getAllProjectsQuery);

            //Quando se busca todos, mesmo que não encontre nenhum o padrão é retornar OK
            return Ok(projects);
        }

        //api/projects/{id}
        [HttpGet("{id}")] //return Ok ou NotFound
        [Authorize(Roles = "client, freelancer")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetById(id);

            if(project == null)
                return NotFound();

            return Ok(project);
        }

        //api/projects
        [HttpPost] //return Created ou BadRequest
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Post([FromBody] CreateProjectCommand command)
        {
            //-------------------------------------------------------------------
            // Validações

            //Criado dentro dos filters
            //if(command.Title.Length > 50)
            //    return BadRequest();

            // Movido para ValidatorFilters para não precisar repetir código
            //if (!ModelState.IsValid)
            //{
            //    var messages = ModelState
            //        .SelectMany(ms => ms.Value.Errors)
            //        .Select(e => e.ErrorMessage)
            //        .ToList();

            //    return BadRequest(messages);
            //}
            //-------------------------------------------------------------------

            // Usando Services
            //var id = await _projectService.Create(inputModel);

            // Usando CQRS
            // O mediatR controla o acesso a outras dependencias / Ele acha para onde ele deve delegar
            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = id }, command);
        }

        //api/projects/{id}
        [HttpPut("{id}")] //return NoContent, NotFound ou BadRequest
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProjectCommand command)
        {
            //Criado dentro dos filters
            //if (command.Description.Length > 200)
            //    return BadRequest();

            // Usando Services
            //await _projectService.Update(inputModel);

            // Usando CQRS
            await _mediator.Send(command);

            return NoContent();
        }

        //api/projetcs/{id}
        [HttpDelete("{id}")] //return NoContent ou NotFound
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Delete(int id)
        {
            //Busca, se nao existir retorna Not Found
            //return NotFound();

            // Usando Services
            //await _projectService.Delete(id);

            // Usando CQRS
            var command = new DeleteProjectCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }

        //---------- Comentarios ----------
        //api/projects/{id}/comments
        [HttpPost("{id}/comment")]
        [Authorize(Roles = "client, freelancer")]
        public async Task<IActionResult> PostComment([FromBody] CreateCommentCommand command)
        {
            // Usando services
            //await _projectService.CreateComment(inputModel);

            // Usando CQRS
            await _mediator.Send(command);

            return NoContent();
        }
        
        //---------- Inicialização/Finalização ----------
        //api/projects/{id}/start
        [HttpPut("{id}/start")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> PutStart(int id)
        {
            await _projectService.Start(id);

            return NoContent();
        }

        //api/projects/{id}/start
        [HttpPut("{id}/finish")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> PutFinish(int id, [FromBody] FinishProjectInputModel inputModel)
        {
            inputModel.Id = id;

            var response = await _projectService.Finish(inputModel);

            if(!response)
                return BadRequest("O pagamento não pôde ser processado.");

            return Accepted();
        }
    }
}