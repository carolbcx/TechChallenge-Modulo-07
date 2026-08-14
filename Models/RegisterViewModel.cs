using System.ComponentModel.DataAnnotations;

namespace TechChallenge;

public class RegisterViewModel
{
    [Required] public string Nome { get; set; }
    [Required][EmailAddress] public string Email { get; set; }
    [Required][DataType(DataType.Password)] public string Senha { get; set; }
    [Compare("Senha")] public string ConfirmarSenha { get; set; }
    [Required] public string Role { get; set; } // Aluno ou Professor
}
