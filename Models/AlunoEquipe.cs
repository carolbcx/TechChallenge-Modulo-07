namespace TechChallenge.Models;

public class AlunoEquipe
{
    public int AlunoId { get; set; }
    public Aluno? Aluno { get; set; }
    public int EquipeId { get; set; }
    public Equipe? Equipe { get; set; }
    public DateTime DataEntrada { get; set; } = DateTime.Now;
}