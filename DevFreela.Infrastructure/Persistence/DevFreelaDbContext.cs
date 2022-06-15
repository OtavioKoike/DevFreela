using DevFreela.Core.Entities;

namespace DevFreela.Infrastructure.Persistence
{
    public class DevFreelaDbContext
    {
        public DevFreelaDbContext()
        {
            Projects = new List<Project>(){
                new Project("Meu projeto ASPNET Core 1", "Minha descrição de Projeto 1", 1, 1, 10000),
                new Project("Meu projeto ASPNET Core 2", "Minha descrição de Projeto 2", 1, 1, 20000),
                new Project("Meu projeto ASPNET Core 3", "Minha descrição de Projeto 3", 1, 1, 30000)
            };

            Users = new List<User>(){
                new User("Otavio Koike", "otaviokoike@hotmail.com", new DateTime(1998, 10, 7)),
                new User("Samantha Aquino", "samanthaaquino@hotmail.com", new DateTime(1997, 9, 18)),
                new User("Luis Felipe", "luisdev@luisdev.com", new DateTime(1992, 1, 1))
            };

            Skills = new List<Skill>(){
                new Skill(".NET Core"),
                new Skill("C#"),
                new Skill("Angular")
            };

            ProjectComments = new List<ProjectComment>(){
                new ProjectComment("Quando posso comecar?", 1, 1)
            };
        }
        public List<Project> Projects { get; set; }
        public List<User> Users { get; set; }
        public List<Skill> Skills { get; set; }
        public List<ProjectComment> ProjectComments { get; set; }
    }
}