using DevFreela.Application.Services.Implementations;
using DevFreela.Application.Services.Interfaces;

namespace DevFreela.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISkillService, SkillService>();

            return services;
        }
    }
}
