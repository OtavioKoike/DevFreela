namespace DevFreela.Core.Entities
{
    //Classe utilizada pois users x skills é n para n
    public class UserSkill: EntityBase
    {
        public UserSkill(int idUser, int idSkill)
        {
            IdUser = idUser;
            IdSkill = idSkill;
        }

        public int IdUser { get; private set; }
        public int IdSkill { get; private set; }
        public Skill Skill { get; private set; } //Propriedade de Navegação
    }
}