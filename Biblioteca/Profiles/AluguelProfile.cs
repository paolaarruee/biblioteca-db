using AutoMapper;
using Biblioteca.Data.Dtos.AluguelDto;
using Biblioteca.Model;

namespace Biblioteca.Profiles
{
    public class AluguelProfile : Profile
    {

        public AluguelProfile()
        {
            CreateMap<CreateAluguelDto, Aluguel>();
            CreateMap<UpdateAluguelDto, Aluguel>();
            CreateMap<Aluguel, UpdateAluguelDto>();
            CreateMap<Aluguel, ReadAluguelDto>();
        }

    }
}
