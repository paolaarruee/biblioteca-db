using AutoMapper;
using Biblioteca.Data;
using Biblioteca.Model;
using Biblioteca.Data.Dtos.LocatarioDto;
using Biblioteca.Data.Dtos.LivroDto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services;

public class LocatarioService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;

    public LocatarioService(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Locatario AdicionarLocatario(CreateLocatarioDto locatarioDto)
    {
        Locatario locatario = _mapper.Map<Locatario>(locatarioDto);
        _context.Locatario.Add(locatario);
        _context.SaveChanges();
        return locatario;
    }

    public List<ReadLocatarioDto> RecuperarLocatarios(int skip = 0, int take = 50)
    {
        var locatarios = _context.Locatario.Skip(skip).Take(take).ToList();
        return _mapper.Map<List<ReadLocatarioDto>>(locatarios);
    }

    public ReadLocatarioDto RecuperarLocatarioPorId(int id)
    {
        var locatario = _context.Locatario.FirstOrDefault(l => l.Id == id);
        if (locatario == null) return null;
        return _mapper.Map<ReadLocatarioDto>(locatario);
    }

    public bool AtualizarLocatario(int id, UpdateLocatarioDto locatarioDto)
    {
        var locatario = _context.Locatario.FirstOrDefault(l => l.Id == id);
        if (locatario == null) return false;

        _mapper.Map(locatarioDto, locatario);
        _context.SaveChanges();
        return true;
    }

    public bool AtualizarLocatarioParcial(int id, JsonPatchDocument<UpdateLocatarioDto> patch)
    {
        var locatario = _context.Locatario.FirstOrDefault(l => l.Id == id);
        if (locatario == null) return false;

        var locatarioParaAtualizar = _mapper.Map<UpdateLocatarioDto>(locatario);
        patch.ApplyTo(locatarioParaAtualizar);

        if (!TryValidateModel(locatarioParaAtualizar))
        {
            return false;
        }

        _mapper.Map(locatarioParaAtualizar, locatario);
        _context.SaveChanges();
        return true;
    }

    public bool DeletarLocatario(int id)
    {
        var locatario = _context.Locatario.FirstOrDefault(l => l.Id == id);
        if (locatario == null) return false;

        bool locatarioTemLivros = _context.Aluguel.Any(aluguel => aluguel.LocatarioId == id && aluguel.Devolucao == null);
        if (locatarioTemLivros) return false;

        _context.Remove(locatario);
        _context.SaveChanges();
        return true;
    }

    public List<ReadLivroDto> RecuperarLivrosAlugadosPorLocatario(int id)
    {
        var locatario = _context.Locatario
            .Include(l => l.Alugueis)
            .ThenInclude(a => a.Livros)
            .FirstOrDefault(l => l.Id == id);

        if (locatario == null) return null;

        var livrosAlugados = locatario.Alugueis.SelectMany(a => a.Livros).ToList();
        return _mapper.Map<List<ReadLivroDto>>(livrosAlugados);
    }

    private bool TryValidateModel(UpdateLocatarioDto locatarioParaAtualizar)
    {
        // Implementação da validação aqui, se necessário
        return true;
    }
}
