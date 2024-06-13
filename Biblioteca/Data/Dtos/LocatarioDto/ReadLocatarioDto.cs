using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Data.Dtos.LocatarioDto;


public class ReadLocatarioDto
{
    public string Nome { get; set; }
    public string Sexo { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string DataNascimento { get; set; }
    public string Cpf { get; set; }
    public DateTime HoraDaConsulta { get; set; } = DateTime.Now;
}

