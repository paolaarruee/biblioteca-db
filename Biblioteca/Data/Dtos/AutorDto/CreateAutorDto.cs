using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Data.Dtos.AutorDto;

public class CreateAutorDto
{
 
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "O sexo é obrigatório")]
    public string Sexo { get; set; }
    [Required(ErrorMessage = "A data de nascimento é obrigatória")]
    public string DataNascimento { get; set; }
    [Required(ErrorMessage = "O CPF é obrigatório")]
    public string Cpf { get; set; }
}
