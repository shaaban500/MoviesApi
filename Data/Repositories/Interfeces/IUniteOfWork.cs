
namespace MoviesApi.Data.Repositories.Interfeces
{
    public interface IUniteOfWork
    {
        IGenresRepository GenreRepository { get; set; }
        IMoviesRepository MoviesRepository { get; set; }        
    }
}
