using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Data.Dtos.AluguelDto;

public class UpdateAluguelDto

{
    [Required(ErrorMessage = "A data de retirada é obrigatoria")]
    public string DataRetirada { get; set; }

    [Required(ErrorMessage = "A devoluição é obrigatoria")]
    public string Devolucao { get; set; }
}
