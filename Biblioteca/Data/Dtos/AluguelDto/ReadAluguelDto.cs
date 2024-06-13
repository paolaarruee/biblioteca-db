using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Data.Dtos.AluguelDto;

public class ReadAluguelDto
{


    public string DataRetirada { get; set; }
    public string Devolucao { get; set; }
    public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
}

