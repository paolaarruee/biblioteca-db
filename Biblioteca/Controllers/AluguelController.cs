using AutoMapper;
using Biblioteca.Data;
using Biblioteca.Model;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.JsonPatch;
using Biblioteca.Data.Dtos.AluguelDto;

namespace Biblioteca.Controllers;



[ApiController]
[Route("[controller]")]
public class AluguelController : ControllerBase
{

    private BibliotecaContext _context;
    private IMapper _mapper;

    public AluguelController(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um aluguel ao banco de dados
    /// </summary>
    /// <param name="aluguelDto">Objeto com os campos necessários para criação de um aluguel</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaLivro(
        [FromBody] CreateAluguelDto aluguelDto)
    {
        Aluguel aluguel = _mapper.Map<Aluguel>(aluguelDto);
        _context.Aluguel.Add(aluguel);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaAluguelPorId),
            new { id = aluguel.Id },
            aluguel);
    }

    [HttpGet]
    public IEnumerable<ReadAluguelDto> RecuperaAluguel([FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadAluguelDto>>(_context.Aluguel.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaAluguelPorId(int id)
    {
        var aluguel = _context.Aluguel
            .FirstOrDefault(aluguel => aluguel.Id == id);
        if (aluguel == null) return NotFound();
        var aluguelDto = _mapper.Map<ReadAluguelDto>(aluguel);
        return Ok(aluguelDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaAluguel(int id,
        [FromBody] UpdateAluguelDto aluguelDto)
    {
        var aluguel = _context.Aluguel.FirstOrDefault(
            aluguel => aluguel.Id == id);
        if (aluguel == null) return NotFound();
        _mapper.Map(aluguelDto, aluguel);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaAluguelParcial(int id,
        JsonPatchDocument<UpdateAluguelDto> patch)
    {
        var aluguel = _context.Aluguel.FirstOrDefault(
            aluguel => aluguel.Id == id);
        if (aluguel == null) return NotFound();

        var aluguelParaAtualizar = _mapper.Map<UpdateAluguelDto>(aluguel);

        patch.ApplyTo(aluguelParaAtualizar, ModelState);

        if (!TryValidateModel(aluguelParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(aluguelParaAtualizar, aluguel);
        _context.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeletaAluguel(int id)
    {
        var aluguel = _context.Aluguel.FirstOrDefault(
            aluguel => aluguel.Id == id);
        if (aluguel == null) return NotFound();
        _context.Remove(aluguel);
        _context.SaveChanges();
        return NoContent();
    }
}

