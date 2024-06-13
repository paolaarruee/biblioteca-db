using AutoMapper;
using Biblioteca.Data;
using Biblioteca.Model;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.JsonPatch;
using Biblioteca.Data.Dtos.LivroDto;

namespace Biblioteca.Controllers;



[ApiController]
[Route("[controller]")]
public class LivroController : ControllerBase
{

    private BibliotecaContext _context;
    private IMapper _mapper;

    public LivroController(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um livro ao banco de dados
    /// </summary>
    /// <param name="livroDto">Objeto com os campos necessários para criação de um livro</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaLivro(
        [FromBody] CreateLivroDto livroDto)
    {
        Livro livro = _mapper.Map<Livro>(livroDto);
        _context.Livro.Add(livro);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaLivroPorId),
            new { id = livro.Id },
            livro);
    }

    [HttpGet]
    public IEnumerable<ReadLivroDto> RecuperaLivro([FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadLivroDto>>(_context.Livro.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaLivroPorId(int id)
    {
        var livro = _context.Livro
            .FirstOrDefault(livro => livro.Id == id);
        if (livro == null) return NotFound();
        var livroDto = _mapper.Map<ReadLivroDto>(livro);
        return Ok(livroDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaLivro(int id,
        [FromBody] UpdateLivroDto filmeDto)
    {
        var livro = _context.Livro.FirstOrDefault(
            livro => livro.Id == id);
        if (livro == null) return NotFound();
        _mapper.Map(filmeDto, livro);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaLivroParcial(int id,
        JsonPatchDocument<UpdateLivroDto> patch)
    {
        var livro = _context.Livro.FirstOrDefault(
            livro => livro.Id == id);
        if (livro == null) return NotFound();

        var livroParaAtualizar = _mapper.Map<UpdateLivroDto>(livro);

        patch.ApplyTo(livroParaAtualizar, ModelState);

        if (!TryValidateModel(livroParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(livroParaAtualizar, livro);
        _context.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeletaLivro(int id)
    {
        var livro = _context.Livro.FirstOrDefault(
            livro => livro.Id == id);
        if (livro == null) return NotFound();

        bool livroAlugado = _context.Aluguel
            .Any(aluguel => aluguel.Livros.Any(l => l.Id == id));

        if (livroAlugado)
        {
            return BadRequest("Não é possível excluir o livro pois ele já foi alugado.");
        }

        _context.Remove(livro);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Recupera livros disponíveis para aluguel
    /// </summary>
    /// <returns>Lista de livros disponíveis</returns>
    [HttpGet("disponiveis")]
    public IEnumerable<ReadLivroDto> RecuperaLivrosDisponiveis()
    {
        var livrosAlugadosIds = _context.Aluguel
            .Where(aluguel => aluguel.Devolucao == null)
            .SelectMany(aluguel => aluguel.Livros.Select(l => l.Id))
            .Distinct()
            .ToList();

        var livrosDisponiveis = _context.Livro
            .Where(livro => !livrosAlugadosIds.Contains(livro.Id))
            .ToList();

        return _mapper.Map<List<ReadLivroDto>>(livrosDisponiveis);
    }

    /// <summary>
    /// Recupera livros alugados
    /// </summary>
    /// <returns>Lista de livros alugados</returns>
    [HttpGet("alugados")]
    public IEnumerable<ReadLivroDto> RecuperaLivrosAlugados()
    {
        var livrosAlugadosIds = _context.Aluguel
            .Where(aluguel => aluguel.Devolucao == null)
            .SelectMany(aluguel => aluguel.Livros.Select(l => l.Id))
            .Distinct()
            .ToList();

        var livrosAlugados = _context.Livro
            .Where(livro => livrosAlugadosIds.Contains(livro.Id))
            .ToList();

        return _mapper.Map<List<ReadLivroDto>>(livrosAlugados);
    }
}




