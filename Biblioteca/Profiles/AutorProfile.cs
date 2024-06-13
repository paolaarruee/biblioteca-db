using AutoMapper;
using Biblioteca.Data.Dtos.AutorDto;
using Biblioteca.Model;

namespace Biblioteca.Profiles
{
    public class AutorProfile : Profile
    {

        public AutorProfile()
        {
            CreateMap<CreateAutorDto, Autor>();
            CreateMap<UpdateAutorDto, Autor>();
            CreateMap<Autor, UpdateAutorDto>();
            CreateMap<Autor, ReadAutorDto>();
        }

    }
}
