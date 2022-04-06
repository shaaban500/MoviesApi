
namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUniteOfWork _context;
        private List<string> _allowedExtensions = new List<string> {".jpg", ".png"};
        private long _maxAllowedPosterSize = 5000000;

        public MoviesController(IUniteOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet] // link will be api/movies/GetAll
        public async Task<IActionResult> GetAllAsync()
        {
            // return as a complex obj || you can return in new MovieDetailsDto to in Genre as attributes in DTO obj 
            var movies = await _context.MoviesRepository.GetAll();
            return Ok(movies);
        }


        [HttpGet("{id}")] // link will be: api/movies/GetById/1
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.MoviesRepository.GetById(id);

            if(movie == null)
                return NotFound();

            return Ok(movie);
        }


        [HttpGet("GetByGenreId")] // link will be: api/movies/GetByGenreId?genreId=1
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _context.MoviesRepository.GetAll(genreId);
            return Ok(movies);
        }


        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required!");

            // take file as formfile not array of byte
            // two things to care about in files:
            // 1. file extension
            // 2. file size
            
            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .jpg and png images are allowed!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size 5MB!");

            // here take a file from user
            // check if its extension is image or not
            // check if size allowed or not
            // copy file in memorystream then .to array to make it byte[]

            var isValidGenre = await _context.GenreRepository.IsvalidGenre(dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genre ID!");


            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();

            _context.MoviesRepository.Add(movie);

            return Ok(movie);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _context.MoviesRepository.GetById(id);

            if (movie == null)
                return NotFound($"No movie was found by ID: {id}");

            var isValidGenre = await _context.GenreRepository.IsvalidGenre(dto.GenreId);
            
            if (!isValidGenre)
                return BadRequest("Invalid genre ID!");


            if (dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .jpg and png images are allowed!");

                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size 5MB!");

                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            
            }

            movie.GenreId = dto.GenreId;
            movie.Title = dto.Title;
            movie.Rate = dto.Rate;
            movie.Storeline = dto.Storeline;
            movie.Year = dto.Year;
            
            _context.MoviesRepository.Update(movie);

            return Ok(movie);

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var movie = await _context.MoviesRepository.GetById(id);

            if (movie == null)
                return NotFound($"No movie was found by ID: {id}");

            _context.MoviesRepository.Delete(movie);
          
            return Ok(movie);
        }


    }
}
