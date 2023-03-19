using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Application.Queries.GetAllProjects
{
    public class GetAllPojectsQueryHandler : IRequestHandler<GetAllPojectsQuery, List<ProjectViewModel>>
    {
        private readonly DevFreelaDbContext _dbContext;

        public GetAllPojectsQueryHandler(DevFreelaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProjectViewModel>> Handle(GetAllPojectsQuery request, CancellationToken cancellationToken)
        {
            var projects = _dbContext.Projects;

            return await projects
                .Select(p => new ProjectViewModel(p.Id, p.Title, p.CreateAt))
                .ToListAsync();
        }
    }
}
