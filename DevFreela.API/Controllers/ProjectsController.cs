using DevFreela.API.Example;
using DevFreela.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Controllers
{
    [Route("api/projects")]
    public class ProjectsController : ControllerBase
    {
        private readonly OpeningTimeOption _option;
        public ProjectsController(IOptions<OpeningTimeOption> option, ExampleClass example)
        {
            example.Name = "Atualizado no Projects Controller";
            _option = option.Value;
        }

        //api/projects?query=netcore
        [HttpGet] //return Ok sempre
        public IActionResult Get(string query)
        {
            //Buscar todos ou filtrar

            //Quando se busca todos, mesmo que não encontre nenhum o padrão é retornar OK
            return Ok();
        }

        //api/projects/{id}
        [HttpGet("{id}")] //return Ok ou NotFound
        public IActionResult GetById(int id)
        {
            //Buscar o projeto

            return Ok();
            //return NotFound();
        }

        //api/projects
        [HttpPost] //return Created ou BadRequest
        public IActionResult Post([FromBody] CreateProjectModel createProject)
        {
            if(createProject.Title.Length > 50)
            {
                return BadRequest();
            }

            //Cadastrar o projeto

            return CreatedAtAction(nameof(GetById), new { id = createProject.Id }, createProject);
        }

        //api/projects/{id}
        [HttpPut("{id}")] //return NoContent, NotFound ou BadRequest
        public IActionResult Put(int id, [FromBody] UpdateProjectModel updateProject)
        {
            if (updateProject.Description.Length > 200)
            {
                return BadRequest();
            }

            //Atualizo o projeto

            return NoContent();
        }

        //api/projetcs/{id}
        [HttpDelete("{id}")] //return NoContent ou NotFound
        public IActionResult Delete(int id)
        {
            //Busca, se nao existir retorna Not Found
            //return NotFound();

            //Remover

            return NoContent();
        }

        //---------- Comentarios ----------
        //api/projects/{id}/comments
        [HttpPost("{id}")]
        public IActionResult PostComment([FromBody] CreateCommentModel createComment)
        {
            return NoContent();
        }
        
        //---------- Inicialização/Finalização ----------
        //api/projects/{id}/start
        [HttpPut("{id}/start")]
        public IActionResult PutStart(int id)
        {
            return NoContent();
        }

        //api/projects/{id}/start
        [HttpPut("{id}/finish")]
        public IActionResult PutFinish(int id)
        {
            return NoContent();
        }
    }
}