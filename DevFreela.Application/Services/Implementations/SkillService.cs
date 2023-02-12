using Dapper;
using DevFreela.Application.Services.Interfaces;
using DevFreela.Application.ViewModels;
using DevFreela.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DevFreela.Application.Services.Implementations
{
    public class SkillService : ISkillService
    {
        private readonly DevFreelaDbContext _dbContext;
        private readonly string _connectionString;
        public SkillService(DevFreelaDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _connectionString = configuration.GetConnectionString("DevFreelaCs");
        }

        public List<SkillViewModel> GetAll()
        {
            // Dapper
            using(var sqlConnection = new SqlConnection(_connectionString)){
                sqlConnection.Open();

                var script = "SELECT Id, Description FROM Skills";
                return sqlConnection.Query<SkillViewModel>(script).ToList();
            }

            // Entity Framework Core
            // var skills = _dbContext.Skills;
            // return skills.Select(s => new SkillViewModel(s.Id, s.Description)).ToList();
        }
    }
}