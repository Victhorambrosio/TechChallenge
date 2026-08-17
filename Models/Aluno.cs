using System.ComponentModel.DataAnnotations;

namespace TechChallenge;

public class Aluno
{
    [Required (ErrorMessage = "O Campo ID é obrigatório")]
    public int Id { get; set; } // Primary Key
    [StringLength(100, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
    public string Nome { get; set; }
    public string Email { get; set; }
    [EmailAddress (ErrorMessage = "Email inválido!")]
    [Required (ErrorMessage ="O campo EMAIL é obrigatório!")]
    public string Telefone { get; set; }
    [Required (ErrorMessage ="O campo TELEFONE é obrigatório!")]
    [Range(1000000000, 99999999999)]
    public DateTime DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }
}
