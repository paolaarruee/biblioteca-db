using AutoMapper;
using Biblioteca.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Biblioteca.Data.Dtos.AutorDto;
using Biblioteca.Services;

namespace Biblioteca.Controllers;

[ApiController]
[Route("[controller]")]
public class AutorController : ControllerBase
{
    private readonly AutorService _autorService;

    public AutorController(AutorService autorService)
    {
        _autorService = autorService;
    }

    /// <summary>
    /// Adiciona um autor ao banco de dados
    /// </summary>
    /// <param name="autorDto">Objeto com os campos necessários para criação de um autor</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaAutor([FromBody] CreateAutorDto autorDto)
    {
        var autor = _autorService.AdicionarAutor(autorDto);
        return CreatedAtAction(nameof(RecuperaAutorPorId), new { id = autor.Id }, autor);
    }

    [HttpGet]
    public IActionResult RecuperaAutor([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var autores = _autorService.RecuperarAutores(skip, take);
        return Ok(autores);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaAutorPorId(int id)
    {
        var autor = _autorService.RecuperarAutorPorId(id);
        if (autor == null) return NotFound();
        return Ok(autor);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaAutor(int id, [FromBody] UpdateAutorDto autorDto)
    {
        if (!_autorService.AtualizarAutor(id, autorDto)) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaAutorParcial(int id, JsonPatchDocument<UpdateAutorDto> patch)
    {
        if (!_autorService.AtualizarAutorParcial(id, patch)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaAutor(int id)
    {
        if (!_autorService.DeletarAutor(id))
            return BadRequest("Não é possível excluir o autor pois ele possui livros associados.");
        return NoContent();
    }

    /// <summary>
    /// Busca autores pelo nome
    /// </summary>
    /// <param name="nome">Nome do autor</param>
    /// <returns>Lista de autores com o nome fornecido</returns>
    [HttpGet("buscarPorNome")]
    public IActionResult RecuperaAutorPorNome([FromQuery] string nome)
    {
        var autores = _autorService.RecuperarAutorPorNome(nome);
        if (autores == null || autores.Count == 0) return NotFound();
        return Ok(autores);
    }
}
