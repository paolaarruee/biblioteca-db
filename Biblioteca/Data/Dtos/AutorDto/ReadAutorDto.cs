using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Data.Dtos.AutorDto;

public class ReadAutorDto
{

  
    public string Nome { get; set; }
  
    public string Sexo { get; set; }
    
    public string DataNascimento { get; set; }
   
    public string Cpf { get; set; }

    public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
}

