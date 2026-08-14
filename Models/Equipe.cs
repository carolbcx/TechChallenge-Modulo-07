using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Models;
public class Equipe
{
    public int Id { get; set; }
    [Required][StringLength(50)] public string Nome { get; set; }
    [StringLength(200)] public string? Descricao { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.Now;
    public bool Ativa { get; set; } = true;
    public ICollection<AlunoEquipe> AlunosEquipes { get; set; } = new List<AlunoEquipe>();
    public ICollection<Projeto> Projetos { get; set; } = new List<Projeto>();
}