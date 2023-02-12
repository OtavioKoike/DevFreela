namespace DevFreela.Core.Entities
{
    public class ProjectComment: EntityBase
    {
        public ProjectComment(string content, int idProject, int idUser)
        {
            Content = content;
            IdProject = idProject;
            IdUser = idUser;
        }

        public string Content { get; private set; }
        public int IdProject { get; private set; }
        public Project Project { get; private set; } //Propriedade de Navegação
        public int IdUser { get; private set; }
        public User User { get; private set; } //Propriedade de Navegação
        public DateTime CreateAt { get; private set; }
    }
}