using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Data.Dtos.LivroDto;

public class ReadLivroDto
{

    public string Nome { get; set; }

    public string Isbn { get; set; }

    public string DataPublicacao { get; set; }

    public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
}

