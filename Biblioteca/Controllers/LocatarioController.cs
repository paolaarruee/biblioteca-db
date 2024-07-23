using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Biblioteca.Data.Dtos.LocatarioDto;
using Biblioteca.Services;
using Biblioteca.Data.Dtos.LivroDto;

namespace Biblioteca.Controllers;

[ApiController]
[Route("[controller]")]
public class LocatarioController : ControllerBase
{
    private readonly LocatarioService _locatarioService;

    public LocatarioController(LocatarioService locatarioService)
    {
        _locatarioService = locatarioService;
    }

    /// <summary>
    /// Adiciona um locatário ao banco de dados
    /// </summary>
    /// <param name="locatarioDto">Objeto com os campos necessários para criação de um locatário</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaLocatario([FromBody] CreateLocatarioDto locatarioDto)
    {
        var locatario = _locatarioService.AdicionarLocatario(locatarioDto);
        return CreatedAtAction(nameof(RecuperaLocatarioPorId), new { id = locatario.Id }, locatario);
    }

    [HttpGet]
    public IActionResult RecuperaLocatario([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var locatarios = _locatarioService.RecuperarLocatarios(skip, take);
        return Ok(locatarios);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaLocatarioPorId(int id)
    {
        var locatario = _locatarioService.RecuperarLocatarioPorId(id);
        if (locatario == null) return NotFound();
        return Ok(locatario);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaLocatario(int id, [FromBody] UpdateLocatarioDto locatarioDto)
    {
        if (!_locatarioService.AtualizarLocatario(id, locatarioDto)) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaLocatarioParcial(int id, JsonPatchDocument<UpdateLocatarioDto> patch)
    {
        if (!_locatarioService.AtualizarLocatarioParcial(id, patch)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaLocatario(int id)
    {
        if (!_locatarioService.DeletarLocatario(id))
            return BadRequest("Não é possível excluir o locatário pois ele possui livros a devolver.");
        return NoContent();
    }

    /// <summary>
    /// Lista todos os livros alugados por um locatário específico
    /// </summary>
    /// <param name="id">ID do locatário</param>
    /// <returns>Lista de livros alugados</returns>
    [HttpGet("{id}/livros-alugados")]
    public IActionResult RecuperaLivrosAlugadosPorLocatario(int id)
    {
        var livrosAlugados = _locatarioService.RecuperarLivrosAlugadosPorLocatario(id);
        if (livrosAlugados == null) return NotFound();
        return Ok(livrosAlugados);
    }
}
