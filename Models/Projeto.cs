using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Models;

public class Projeto
{
    public int Id { get; set; }
    [Required][StringLength(100)] public string Nome { get; set; }
    [StringLength(500)] public string? Descricao { get; set; }
    [DataType(DataType.Date)] public DateTime DataInicio { get; set; }
    [DataType(DataType.Date)] public DateTime? DataFim { get; set; }
    public int Pontuacao { get; set; }
    public int ProfessorId { get; set; }
    public int CategoriaId { get; set; }
    public int EquipeId { get; set; }
    public Professor? Professor { get; set; }
    public Categoria? Categoria { get; set; }
    public Equipe? Equipe { get; set; }
}