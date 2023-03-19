using DevFreela.Application.ViewModels;
using MediatR;

namespace DevFreela.Application.Queries.GetAllProjects
{
    public class GetAllPojectsQuery : IRequest<List<ProjectViewModel>>
    {
        public string Query { get; set; }

        public GetAllPojectsQuery(string query)
        {
            Query = query;
        }
    }
}
