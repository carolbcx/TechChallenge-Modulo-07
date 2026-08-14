using System.ComponentModel.DataAnnotations;
using TechChallenge.Models;

namespace TechChallenge;

public class Professor
{
    public int Id { get; set; }
    [Required][StringLength(100)] public string Nome { get; set; }
    [Required][EmailAddress] public string Email { get; set; }
    [Required][StringLength(50)] public string Especialidade { get; set; }
    [DataType(DataType.Date)] public DateTime DataContratacao { get; set; }
    public bool Ativo { get; set; } = true;
    public ICollection<Projeto>? Projetos { get; set; } = new List<Projeto>();
}