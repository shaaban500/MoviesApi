
namespace MoviesApi.Data.Repositories.Implementations
{
    public class UniteOfWork : IUniteOfWork
    {
        public IGenresRepository GenreRepository { get; set; }
        public IMoviesRepository MoviesRepository { get; set; }

        public UniteOfWork(AppDbContext context)
        {
            GenreRepository = new GenresRepository(context);
            MoviesRepository = new MoviesRepository(context);
        }

        
    }
}
