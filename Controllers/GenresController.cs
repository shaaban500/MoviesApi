
namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUniteOfWork _context;
        public GenresController(IUniteOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var genres = await _context.GenreRepository.GetAll();
            return Ok(genres);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(GenreDto dto)
        {
            var genre = _mapper.Map<Genre>(dto);
            await _context.GenreRepository.Add(genre);
            return Ok(genre);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id, [FromBody] GenreDto dto)
        {
            var genre = await _context.GenreRepository.GetById(id);

            if (genre == null)
                return NotFound($"No genre was found with ID : {id}");

            genre.Name = dto.Name;
            _context.GenreRepository.Update(genre);

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _context.GenreRepository.GetById(id);

            if (genre == null)
                return NotFound($"No genre was found with ID : {id}");

            _context.GenreRepository.Delete(genre);

            return Ok(genre);

        }
    }
}
