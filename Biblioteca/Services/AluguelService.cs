using AutoMapper;
using Biblioteca.Data;
using Biblioteca.Model;
using Biblioteca.Data.Dtos.AluguelDto;
using Microsoft.AspNetCore.JsonPatch;

namespace Biblioteca.Services
{
    public class AluguelService
    {
        private readonly BibliotecaContext _context;
        private readonly IMapper _mapper;

        public AluguelService(BibliotecaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IMapper Mapper => _mapper;
        public BibliotecaContext Context => _context;

        public Aluguel AdicionaAluguel(CreateAluguelDto aluguelDto)
        {
            Aluguel aluguel = _mapper.Map<Aluguel>(aluguelDto);
            _context.Aluguel.Add(aluguel);
            _context.SaveChanges();
            return aluguel;
        }

        public List<ReadAluguelDto> RecuperaAlugueis(int skip, int take)
        {
            return _mapper.Map<List<ReadAluguelDto>>(_context.Aluguel.Skip(skip).Take(take).ToList());
        }

        public Aluguel RecuperaAluguelPorId(int id)
        {
            return _context.Aluguel.FirstOrDefault(aluguel => aluguel.Id == id);
        }

        public bool AtualizaAluguel(int id, UpdateAluguelDto aluguelDto)
        {
            var aluguel = _context.Aluguel.FirstOrDefault(aluguel => aluguel.Id == id);
            if (aluguel == null) return false;
            _mapper.Map(aluguelDto, aluguel);
            _context.SaveChanges();
            return true;
        }

        public bool AtualizaAluguelParcial(int id, JsonPatchDocument<UpdateAluguelDto> patch, out UpdateAluguelDto aluguelParaAtualizar)
        {
            var aluguel = _context.Aluguel.FirstOrDefault(aluguel => aluguel.Id == id);
            if (aluguel == null)
            {
                aluguelParaAtualizar = null;
                return false;
            }

            aluguelParaAtualizar = _mapper.Map<UpdateAluguelDto>(aluguel);
            patch.ApplyTo(aluguelParaAtualizar);
            return true;
        }

        public bool DeletaAluguel(int id)
        {
            var aluguel = _context.Aluguel.FirstOrDefault(aluguel => aluguel.Id == id);
            if (aluguel == null) return false;
            _context.Remove(aluguel);
            _context.SaveChanges();
            return true;
        }
    }
}
