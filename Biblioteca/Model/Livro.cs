using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Model;

public class Livro
{
    public int Id { get; set; }
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "O ISBN é obrigatório")]
    public string Isbn { get; set; }
    [Required(ErrorMessage = "O data publicação é obrigatória")]
    public string DataPublicacao { get; set; }

    // Propriedades de Navegação
    public ICollection<Autor> Autores { get; set; }
    public ICollection<Aluguel> Alugueis { get; set; }
}
