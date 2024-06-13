using Biblioteca.Model;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> opts) : base(opts) { }

    public DbSet<Livro> Livro { get; set; }
    public DbSet<Aluguel> Aluguel { get; set; }
    public DbSet<Locatario> Locatario { get; set; }
    public DbSet<Autor> Autor { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuração da relação muitos-para-muitos entre Autor e Livro
        modelBuilder.Entity<Autor>()
            .HasMany(a => a.Livros)
            .WithMany(l => l.Autores)
            .UsingEntity(j => j.ToTable("AutorLivro"));

        // Configuração da relação muitos-para-muitos entre Aluguel e Livro
        modelBuilder.Entity<Aluguel>()
            .HasMany(a => a.Livros)
            .WithMany(l => l.Alugueis)
            .UsingEntity(j => j.ToTable("AluguelLivro"));

        // Configuração da relação um-para-muitos entre Aluguel e Locatario
        modelBuilder.Entity<Aluguel>()
            .HasOne(a => a.Locatario)
            .WithMany(l => l.Alugueis)
            .HasForeignKey(a => a.LocatarioId);
    }
}


