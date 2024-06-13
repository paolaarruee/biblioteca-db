﻿using System.ComponentModel.DataAnnotations;


namespace Biblioteca.Data.Dtos.LocatarioDto;

public class UpdateLocatarioDto

{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "O sexo é obrigatório")]
    public string Sexo { get; set; }
    [Required(ErrorMessage = "O telefone é obrigatório")]
    public string Telefone { get; set; }
    [Required(ErrorMessage = "O email é obrigatório")]
    public string Email { get; set; }
    [Required(ErrorMessage = "O data nascimento é obrigatório")]
    public string DataNascimento { get; set; }
    [Required(ErrorMessage = "O Cpf é obrigatório")]
    public string Cpf { get; set; }
}
