using DevFreela.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevFreela.Infrastructure.Persistence.Configurations
{
    class ProjectConfigurations : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            // Definir as chaves primarias
            builder
                // .ToTable("tb_Project") // Definir nome da Tabela no Banco
                .HasKey(p => p.Id); 

            // Configuração de Relacionamento
            builder
                .HasOne(p => p.Freelancer)  // Um projeto tem um freelancer
                .WithMany(f => f.FreelanceProjects) // Um freelancer tem varios projetos
                .HasForeignKey(p => p.IdFreelancer) // A chave estrangeira é IdFreelancer
                .OnDelete(DeleteBehavior.Restrict); // O Comportamento que vai ocorrer quando remover uma das entidades do relacionamento
        
            builder
                .HasOne(p => p.Client) 
                .WithMany(c => c.OwnedProjects)
                .HasForeignKey(p => p.IdClient)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
