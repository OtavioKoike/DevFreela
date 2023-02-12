namespace DevFreela.Core.Entities
{
    public class User: EntityBase
    {
        public User(string fullName, string email, DateTime birthDate, string password)
        {
            FullName = fullName;
            Email = email;
            BirthDate = birthDate;
            Password = password;
            
            Active = true;            
            CreateAt = DateTime.Now;
            
            Skills = new List<UserSkill>();
            OwnedProjects = new List<Project>();
            FreelanceProjects = new List<Project>();
        }

        public string FullName { get; private set; }
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string Password { get; private set; }
        public bool Active { get; private set; }
        public DateTime CreateAt { get; private set; }
        public List<UserSkill> Skills { get; private set; }
        public List<Project> OwnedProjects { get; private set; }
        public List<Project> FreelanceProjects { get; private set; }
        public List<ProjectComment> Comments { get; private set; } //Propriedade de Navegação
    }
}