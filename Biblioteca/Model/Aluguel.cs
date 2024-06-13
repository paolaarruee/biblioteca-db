using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Model;

public class Aluguel
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage ="A data de retirada é obrigatoria")]
    public string DataRetirada { get; set; }

    [Required(ErrorMessage = "A devoluição é obrigatoria")]
    public string Devolucao { get; set; }

    // Chave estrangeira
    public int LocatarioId { get; set; }

    // Propriedades de Navegação
    public Locatario Locatario { get; set; }
    public ICollection<Livro> Livros { get; set; }
}
