using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Biblioteca.Data.Dtos.LivroDto;
using Biblioteca.Services;

namespace Biblioteca.Controllers;

[ApiController]
[Route("[controller]")]
public class LivroController : ControllerBase
{
    private readonly LivroService _livroService;

    public LivroController(LivroService livroService)
    {
        _livroService = livroService;
    }

    /// <summary>
    /// Adiciona um livro ao banco de dados
    /// </summary>
    /// <param name="livroDto">Objeto com os campos necessários para criação de um livro</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaLivro([FromBody] CreateLivroDto livroDto)
    {
        var livro = _livroService.AdicionarLivro(livroDto);
        return CreatedAtAction(nameof(RecuperaLivroPorId), new { id = livro.Id }, livro);
    }

    [HttpGet]
    public IActionResult RecuperaLivro([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var livros = _livroService.RecuperarLivros(skip, take);
        return Ok(livros);
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaLivroPorId(int id)
    {
        var livro = _livroService.RecuperarLivroPorId(id);
        if (livro == null) return NotFound();
        return Ok(livro);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaLivro(int id, [FromBody] UpdateLivroDto livroDto)
    {
        if (!_livroService.AtualizarLivro(id, livroDto)) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaLivroParcial(int id, JsonPatchDocument<UpdateLivroDto> patch)
    {
        if (!_livroService.AtualizarLivroParcial(id, patch)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaLivro(int id)
    {
        if (!_livroService.DeletarLivro(id))
            return BadRequest("Não é possível excluir o livro pois ele já foi alugado.");
        return NoContent();
    }

    /// <summary>
    /// Recupera livros disponíveis para aluguel
    /// </summary>
    /// <returns>Lista de livros disponíveis</returns>
    [HttpGet("disponiveis")]
    public IActionResult RecuperaLivrosDisponiveis()
    {
        var livrosDisponiveis = _livroService.RecuperarLivrosDisponiveis();
        return Ok(livrosDisponiveis);
    }

    /// <summary>
    /// Recupera livros alugados
    /// </summary>
    /// <returns>Lista de livros alugados</returns>
    [HttpGet("alugados")]
    public IActionResult RecuperaLivrosAlugados()
    {
        var livrosAlugados = _livroService.RecuperarLivrosAlugados();
        return Ok(livrosAlugados);
    }
}
