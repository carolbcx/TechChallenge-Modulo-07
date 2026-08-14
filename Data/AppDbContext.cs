using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechChallenge.Models;

namespace TechChallenge;

public class AppDbContext : IdentityDbContext<IdentityUser>  // Herança do Contexto do EF Core
{
    // Herança no Método Construtor
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}
    /*
        DbSet -> Representação da Tabela no Sistema.
        Para cada model do sistema que for virar uma tabela no Banco de
        de Dados, deverá ser adicionado um DbSet
    */
    public DbSet<Aluno> Alunos {get; set;}  
    public DbSet<Categoria> Categorias {get; set;} 
    public DbSet<Professor> Professores { get; set; }
    public DbSet<Equipe> Equipes { get; set; }
    public DbSet<AlunoEquipe> AlunosEquipes { get; set; }
    public DbSet<Projeto> Projetos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AlunoEquipe>().HasKey(ae => new { ae.AlunoId, ae.EquipeId });
    }
}
