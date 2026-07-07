using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Tabelas do banco de dados mapeadas pelo Entity Framework

        public DbSet<Pessoa> Pessoas { get; set; }

        public DbSet<Transacao> Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapeia que uma Transação obrigatoriamente pertence a uma Pessoa
            modelBuilder.Entity<Transacao>()
                .HasOne<Pessoa>() //Cada transação pertence à uma pessoa
                .WithMany() // Uma pessoa pode ter muitas transações
                .HasForeignKey(t => t.PessoaId) //Usar o campo Pessoa Id para identificar à quem pertence à transação
                .OnDelete(DeleteBehavior.Cascade); // Se deletar a pessoa, deleta as transações dela
        }
    }
}