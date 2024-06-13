using AutoMapper;
using Biblioteca.Data;
using Biblioteca.Model;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.JsonPatch;
using Biblioteca.Data.Dtos.AutorDto;



namespace Biblioteca.Controllers;

[ApiController]
[Route("[controller]")]
public class AutorController : ControllerBase
{

    private BibliotecaContext _context;
    private IMapper _mapper;

    public AutorController(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um autor ao banco de dados
    /// </summary>
    /// <param name="autorDto">Objeto com os campos necessários para criação de um autor</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaAutor(
        [FromBody] CreateAutorDto autorDto)
    {
        Autor autor = _mapper.Map<Autor>(autorDto);
        _context.Autor.Add(autor);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaAutorPorId),
            new { id = autor.Id },
            autor);
    }

    [HttpGet]
    public IEnumerable<ReadAutorDto> RecuperaAutor([FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        return _mapper.Map<List<ReadAutorDto>>(_context.Autor.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult RecuperaAutorPorId(int id)
    {
        var autor = _context.Autor
            .FirstOrDefault(autor => autor.Id == id);
        if (autor == null) return NotFound();
        var autorDto = _mapper.Map<ReadAutorDto>(autor);
        return Ok(autorDto);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaAutor(int id,
        [FromBody] UpdateAutorDto autorDto)
    {
        var autor = _context.Autor.FirstOrDefault(
            autor => autor.Id == id);
        if (autor == null) return NotFound();
        _mapper.Map(autorDto, autor);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizaAutorParcial(int id,
        JsonPatchDocument<UpdateAutorDto> patch)
    {
        var autor = _context.Autor.FirstOrDefault(
            autor => autor.Id == id);
        if (autor == null) return NotFound();

        var autorParaAtualizar = _mapper.Map<UpdateAutorDto>(autor);

        patch.ApplyTo(autorParaAtualizar, ModelState);

        if (!TryValidateModel(autorParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(autorParaAtualizar, autor);
        _context.SaveChanges();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public IActionResult DeletaAutor(int id)
    {
        var autor = _context.Autor.FirstOrDefault(
            autor => autor.Id == id);
        if (autor == null) return NotFound();

        // Verificar se o autor possui livros associados
        bool autorTemLivros = _context.Livro
            .Any(livro => livro.Autores.Any(a => a.Id == id));

        if (autorTemLivros)
        {
            return BadRequest("Não é possível excluir o autor pois ele possui livros associados.");
        }
        _context.Remove(autor);
        _context.SaveChanges();
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
        var autores = _context.Autor.Where(autor => autor.Nome.Contains(nome)).ToList();
        if (autores == null || autores.Count == 0) return NotFound();
        var autorDtos = _mapper.Map<List<ReadAutorDto>>(autores);
        return Ok(autorDtos);
    }
}

