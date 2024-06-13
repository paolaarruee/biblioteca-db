using AutoMapper;
using Biblioteca.Data.Dtos.LivroDto;
using Biblioteca.Model;

namespace Biblioteca.Profiles;

public class LivroProfile : Profile
{
    
        public LivroProfile()
        {
            CreateMap<CreateLivroDto, Livro>();
            CreateMap<UpdateLivroDto, Livro>();
            CreateMap<Livro, UpdateLivroDto>();
            CreateMap<Livro, ReadLivroDto>();
    }
    
}
