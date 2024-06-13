using AutoMapper;
using Biblioteca.Data;
using Biblioteca.Model;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.JsonPatch;
using Biblioteca.Data.Dtos.LocatarioDto;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using Biblioteca.Data.Dtos.LivroDto;


namespace Biblioteca.Controllers;



[ApiController]
[Route("[controller]")]
public class LocatarioController : ControllerBase
{

    private BibliotecaContext _context;
    private IMapper _mapper;

    public LocatarioController(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um locatario ao banco de dados
    /// </summary>
    /// <param name="locatarioDto">Objeto com os campos necessários para criação de um locatario</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaLocatario(
        [FromBody] CreateLocatarioDto locatarioDto)
    {
        Locatario locatario = _mapper.Map<Locatario>(locatarioDto);
        _context.Locatario.Add(locatario);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaLocatarioPorId),
            new { id = locatario.Id },
            locatario);
    }

    [HttpGet]
    public IEnumerable<ReadLocatarioDto> RecuperaLocatario([FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadLocatarioDto>>(_context.Locatario.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaLocatarioPorId(int id)
    {
        var locatario = _context.Locatario
            .FirstOrDefault(locatario => locatario.Id == id);
        if (locatario == null) return NotFound();
        var locatarioDto = _mapper.Map<ReadLocatarioDto>(locatario);
        return Ok(locatarioDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaLocatario(int id,
        [FromBody] UpdateLocatarioDto locatarioDto)
    {
        var locatario = _context.Locatario.FirstOrDefault(
            locatario => locatario.Id == id);
        if (locatario == null) return NotFound();
        _mapper.Map(locatarioDto, locatario);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaLocatarioParcial(int id,
        JsonPatchDocument<UpdateLocatarioDto> patch)
    {
        var locatario = _context.Locatario.FirstOrDefault(
            locatario => locatario.Id == id);
        if (locatario == null) return NotFound();

        var locatarioParaAtualizar = _mapper.Map<UpdateLocatarioDto>(locatario);

        patch.ApplyTo(locatarioParaAtualizar, ModelState);

        if (!TryValidateModel(locatarioParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(locatarioParaAtualizar, locatario);
        _context.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeletaLocatario(int id)
    {
        var locatario = _context.Locatario.FirstOrDefault(
            locatario => locatario.Id == id);
        if (locatario == null) return NotFound();

        bool locatarioTemLivros = _context.Aluguel
            .Any(aluguel => aluguel.LocatarioId == id && aluguel.Devolucao == null);

        if (locatarioTemLivros)
        {
            return BadRequest("Não é possível excluir o locatário pois ele possui livros a devolver.");
        }

        _context.Remove(locatario);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Lista todos os livros alugados por um locatário específico
    /// </summary>
    /// <param name="locatarioId">ID do locatário</param>
    /// <returns>Lista de livros alugados</returns>
    [HttpGet("{id}/livros-alugados")]
    public IActionResult RecuperaLivrosAlugadosPorLocatario(int id)
    {
        var locatario = _context.Locatario
            .Include(l => l.Alugueis)
            .ThenInclude(a => a.Livros)
            .FirstOrDefault(l => l.Id == id);

        if (locatario == null)
            return NotFound();

        var livrosAlugados = locatario.Alugueis.SelectMany(a => a.Livros).ToList();
        var livrosAlugadosDto = _mapper.Map<List<ReadLivroDto>>(livrosAlugados);

        return Ok(livrosAlugadosDto);
    }
}


