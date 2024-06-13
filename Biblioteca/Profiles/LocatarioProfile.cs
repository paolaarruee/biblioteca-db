using AutoMapper;
using Biblioteca.Data.Dtos.LocatarioDto;
using Biblioteca.Model;

namespace Biblioteca.Profiles
{
    public class LocatarioProfile : Profile
    {

        public LocatarioProfile()
        {
            CreateMap<CreateLocatarioDto, Locatario>();
            CreateMap<UpdateLocatarioDto, Locatario>();
            CreateMap<Locatario, UpdateLocatarioDto>();
            CreateMap<Locatario, ReadLocatarioDto>();
        }

    }
}
