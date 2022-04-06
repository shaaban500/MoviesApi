
namespace MoviesApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MovieDto,Movie>()
                .ForMember(src => src.Poster, opt => opt.Ignore())
                .ReverseMap();
         
            CreateMap<Genre,GenreDto>().ReverseMap();
        }
    }
}
