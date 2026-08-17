using System.ComponentModel.DataAnnotations;

namespace TechChallenge;

public class Aluno
{
    [Required (ErrorMessage = "Ocampo ID é obrigatorio")]public int Id { get; set; } // Primary Key
    [StringLength(100, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set; }
    public DateTime DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }
}
