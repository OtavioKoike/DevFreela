using DevFreela.Application.InputModels;
using DevFreela.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevFreela.API.Controllers
{
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        //api/projects?query=netcore
        [HttpGet] //return Ok sempre
        public IActionResult Get(string query)
        {
            var projects = _projectService.GetAll(query);

            //Quando se busca todos, mesmo que não encontre nenhum o padrão é retornar OK
            return Ok(projects);
        }

        //api/projects/{id}
        [HttpGet("{id}")] //return Ok ou NotFound
        public IActionResult GetById(int id)
        {
            var project = _projectService.GetById(id);

            if(project == null)
                return NotFound();

            return Ok(project);
        }

        //api/projects
        [HttpPost] //return Created ou BadRequest
        public IActionResult Post([FromBody] NewProjectInputModel inputModel)
        {
            if(inputModel.Title.Length > 50)
            {
                return BadRequest();
            }

            var id = _projectService.Create(inputModel);

            return CreatedAtAction(nameof(GetById), new { id = id }, inputModel);
        }

        //api/projects/{id}
        [HttpPut("{id}")] //return NoContent, NotFound ou BadRequest
        public IActionResult Put(int id, [FromBody] UpdateProjectInputModel inputModel)
        {
            if (inputModel.Description.Length > 200)
            {
                return BadRequest();
            }

            _projectService.Update(inputModel);

            return NoContent();
        }

        //api/projetcs/{id}
        [HttpDelete("{id}")] //return NoContent ou NotFound
        public IActionResult Delete(int id)
        {
            //Busca, se nao existir retorna Not Found
            //return NotFound();

            _projectService.Delete(id);

            return NoContent();
        }

        //---------- Comentarios ----------
        //api/projects/{id}/comments
        [HttpPost("{id}")]
        public IActionResult PostComment([FromBody] CreateCommentInputModel inputModel)
        {
            _projectService.CreateComment(inputModel);

            return NoContent();
        }
        
        //---------- Inicialização/Finalização ----------
        //api/projects/{id}/start
        [HttpPut("{id}/start")]
        public IActionResult PutStart(int id)
        {
            _projectService.Start(id);

            return NoContent();
        }

        //api/projects/{id}/start
        [HttpPut("{id}/finish")]
        public IActionResult PutFinish(int id)
        {
            _projectService.Finish(id);

            return NoContent();
        }
    }
}