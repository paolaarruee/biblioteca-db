using AutoMapper;
using Biblioteca.Data;
using Biblioteca.Model;
using Biblioteca.Data.Dtos.LivroDto;
using Microsoft.AspNetCore.JsonPatch;

namespace Biblioteca.Services;

public class LivroService
{
    private readonly BibliotecaContext _context;
    private readonly IMapper _mapper;

    public LivroService(BibliotecaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Livro AdicionarLivro(CreateLivroDto livroDto)
    {
        var livro = _mapper.Map<Livro>(livroDto);
        _context.Livro.Add(livro);
        _context.SaveChanges();
        return livro;
    }

    public List<ReadLivroDto> RecuperarLivros(int skip, int take)
    {
        var livros = _context.Livro.Skip(skip).Take(take).ToList();
        return _mapper.Map<List<ReadLivroDto>>(livros);
    }

    public ReadLivroDto RecuperarLivroPorId(int id)
    {
        var livro = _context.Livro.FirstOrDefault(l => l.Id == id);
        if (livro == null) return null;
        return _mapper.Map<ReadLivroDto>(livro);
    }

    public bool AtualizarLivro(int id, UpdateLivroDto livroDto)
    {
        var livro = _context.Livro.FirstOrDefault(l => l.Id == id);
        if (livro == null) return false;
        _mapper.Map(livroDto, livro);
        _context.SaveChanges();
        return true;
    }

    public bool AtualizarLivroParcial(int id, JsonPatchDocument<UpdateLivroDto> patch)
    {
        var livro = _context.Livro.FirstOrDefault(l => l.Id == id);
        if (livro == null) return false;

        var livroParaAtualizar = _mapper.Map<UpdateLivroDto>(livro);
        patch.ApplyTo(livroParaAtualizar);

        if (!TryValidateModel(livroParaAtualizar))
        {
            return false;
        }

        _mapper.Map(livroParaAtualizar, livro);
        _context.SaveChanges();
        return true;
    }

    public bool DeletarLivro(int id)
    {
        var livro = _context.Livro.FirstOrDefault(l => l.Id == id);
        if (livro == null) return false;

        bool livroAlugado = _context.Aluguel.Any(aluguel => aluguel.Livros.Any(l => l.Id == id));
        if (livroAlugado) return false;

        _context.Remove(livro);
        _context.SaveChanges();
        return true;
    }

    public List<ReadLivroDto> RecuperarLivrosDisponiveis()
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

    public List<ReadLivroDto> RecuperarLivrosAlugados()
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

    private bool TryValidateModel(UpdateLivroDto livroParaAtualizar)
    {
        // Implementação da validação aqui, se necessário
        return true;
    }
}
