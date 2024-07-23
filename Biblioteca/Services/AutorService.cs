using AutoMapper;
using Biblioteca.Data;
using Biblioteca.Model;
using Biblioteca.Data.Dtos.AutorDto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;

namespace Biblioteca.Services;

public class AutorService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;

    public AutorService(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Autor AdicionarAutor(CreateAutorDto autorDto)
    {
        Autor autor = _mapper.Map<Autor>(autorDto);
        _context.Autor.Add(autor);
        _context.SaveChanges();
        return autor;
    }

    public List<ReadAutorDto> RecuperarAutores(int skip, int take)
    {
        var autores = _context.Autor.Skip(skip).Take(take).ToList();
        return _mapper.Map<List<ReadAutorDto>>(autores);
    }

    public ReadAutorDto RecuperarAutorPorId(int id)
    {
        var autor = _context.Autor.FirstOrDefault(a => a.Id == id);
        if (autor == null) return null;
        return _mapper.Map<ReadAutorDto>(autor);
    }

    public bool AtualizarAutor(int id, UpdateAutorDto autorDto)
    {
        var autor = _context.Autor.FirstOrDefault(a => a.Id == id);
        if (autor == null) return false;
        _mapper.Map(autorDto, autor);
        _context.SaveChanges();
        return true;
    }

    public bool AtualizarAutorParcial(int id, JsonPatchDocument<UpdateAutorDto> patch)
    {
        var autor = _context.Autor.FirstOrDefault(a => a.Id == id);
        if (autor == null) return false;

        var autorParaAtualizar = _mapper.Map<UpdateAutorDto>(autor);
        patch.ApplyTo(autorParaAtualizar);

        if (!TryValidateModel(autorParaAtualizar))
        {
            return false;
        }

        _mapper.Map(autorParaAtualizar, autor);
        _context.SaveChanges();
        return true;
    }

    public bool DeletarAutor(int id)
    {
        var autor = _context.Autor.FirstOrDefault(a => a.Id == id);
        if (autor == null) return false;

        bool autorTemLivros = _context.Livro.Any(livro => livro.Autores.Any(a => a.Id == id));
        if (autorTemLivros) return false;

        _context.Remove(autor);
        _context.SaveChanges();
        return true;
    }

    public List<ReadAutorDto> RecuperarAutorPorNome(string nome)
    {
        var autores = _context.Autor.Where(a => a.Nome.Contains(nome)).ToList();
        return _mapper.Map<List<ReadAutorDto>>(autores);
    }

    private bool TryValidateModel(UpdateAutorDto autorParaAtualizar)
    {
        return true;
    }
}
