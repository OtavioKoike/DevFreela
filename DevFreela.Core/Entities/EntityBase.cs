namespace DevFreela.Core.Entities
{
    public abstract class EntityBase
    {
        protected EntityBase(){}
        public int Id { get; private set; }
    }
}