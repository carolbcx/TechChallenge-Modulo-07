using System.ComponentModel.DataAnnotations;

namespace TechChallenge;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome da categoria é obrigatório")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 50 caracteres")]
    [Display(Name = "Nome da Categoria")]
    public string Nome { get; set; }

    [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
    [Display(Name = "Descrição")]
    public string? Descricao { get; set; }

    [Display(Name = "Cor no Dashboard")]
    public string? Cor { get; set; }  // Ex: "#ff6b6b", "#4ecdc4"

    [Display(Name = "Ativa")]
    public bool Ativa { get; set; } = true;

    public DateTime DataCadastro { get; set; } = DateTime.Now;
}
