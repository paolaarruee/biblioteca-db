using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Data.Dtos.LivroDto;

public class CreateLivroDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "O ISBN é obrigatório")]
    public string Isbn { get; set; }
    [Required(ErrorMessage = "O data publicação é obrigatória")]
    public string DataPublicacao { get; set; }
}
