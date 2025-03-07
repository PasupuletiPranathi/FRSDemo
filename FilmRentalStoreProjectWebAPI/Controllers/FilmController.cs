using FilmRentalStoreProjectDAL.Models;
using FilmRentalStoreProjectREPO.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmRentalStoreProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilm _filmRepo;

        public FilmController(IFilm filmRepo)
        {
            _filmRepo = filmRepo;
        }

        [Authorize(Roles = "Staff")]
        [HttpPost("post")]
        public async Task<IActionResult> AddFilm([FromBody] FilmPostDTO filmDto)
        {
            if (filmDto == null)
            {
                return BadRequest("Film data is null");
            }

            var film = new Film
            {
                Title = filmDto.Title,
                Description = filmDto.Description,
                ReleaseYear = filmDto.ReleaseYear,
                LanguageId = filmDto.LanguageId,
                OriginalLanguageId = filmDto.OriginalLanguageId,
                RentalDuration = filmDto.RentalDuration,
                RentalRate = filmDto.RentalRate,
                Length = filmDto.Length,
                ReplacementCost = filmDto.ReplacementCost,
                Rating = filmDto.Rating,
                SpecialFeatures = filmDto.SpecialFeatures
            };

            await _filmRepo.AddFilmAsync(film);
            return Ok("Record Created Successfully");
        }


        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetFilmsByTitle(string title)
        {
            return Ok(await _filmRepo.GetFilmsByTitleAsync(title));
        }
        [HttpGet("year/{year}")]
        public async Task<IActionResult> GetFilmsByYear(int year)
        {
            return Ok(await _filmRepo.GetFilmsByYear(year));
        }
        [HttpGet("duration/gt/{rd}")]
        public async Task<IActionResult> GetFilmsByRentalDurationGreater(int rd)
        {
            return Ok(await _filmRepo.GetFilmsByRentalDurationGreater(rd));
        }
        [HttpGet("rate/gt/{rate}")]
        public async Task<IActionResult> GetFilmsByRentalRateGreater(decimal rate)
        {
            return Ok(await _filmRepo.GetFilmsByRentalRateGreater(rate));
        }
        [HttpGet("length/gt/{length}")]
        public async Task<IActionResult> GetFilmsByLengthGreater(int length)
        {
            return Ok(await _filmRepo.GetFilmsByLengthGreater(length));
        }
        [HttpGet("duration/lt/{rd}")]
        public async Task<IActionResult> GetFilmsByRentalDurationLower(int rd)
        {
            return Ok(await _filmRepo.GetFilmsByRentalDurationLower(rd));
        }

        [HttpGet("rate/lt/{rate}")]
        public async Task<IActionResult> GetFilmsByRentalRateLower(decimal rate)
        {
            return Ok(await _filmRepo.GetFilmsByRentalRateLower(rate));
        }
        [HttpGet("length/lt/{length}")]
        public async Task<IActionResult> GetFilmsByLength(int length)
        {
            var films = await _filmRepo.GetFilmsByLength(length);
            return Ok(films);
        }


        [HttpGet("betweenyear/{from}/{to}")]
        public async Task<IActionResult> GetFilmsByYearRange(int from, int to)
        {
            var films = await _filmRepo.GetFilmsByYearRange(from, to);
            return Ok(films);
        }

        [HttpGet("rating/lt/{rating}")]
        public async Task<ActionResult<List<Film>>> GetFilmsByRatingLowerThan(string rating)
        {
            var films = await _filmRepo.GetFilmsByRatingLowerThan(rating);

            if (films == null || films.Count == 0)
            {
                return NotFound("No films found with a lower rating.");
            }

            return Ok(films);
        }

        [HttpGet("rating/gt/{rating}")]
        public async Task<ActionResult<List<Film>>> GetFilmsByRatingGreaterThan(string rating)
        {
            var films = await _filmRepo.GetFilmsByRatingGreaterThan(rating);

            if (films == null || films.Count == 0)
            {
                return NotFound("No films found with a greater rating.");
            }

            return Ok(films);
        }

        [HttpGet("countbyyear")]
        public async Task<IActionResult> GetFilmCountByYear()
        {
            var filmCounts = await _filmRepo.GetFilmCountByYearAsync();

            if (!filmCounts.Any())
            {
                return NotFound(new { message = "No films found." });
            }

            return Ok(filmCounts);
        }
        [HttpGet("{id}/actors")]
        public async Task<IActionResult> GetActorsByFilmId(int id)
        {
            var actors = await _filmRepo.GetActorsByFilmIdAsync(id);
            if (actors == null || !actors.Any())
            {
                return NotFound(new { message = "No actors found for this film." });
            }
            return Ok(actors);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetFilmsByCategory(string category)
        {
            var films = await _filmRepo.GetFilmsByCategoryAsync(category);
            if (!films.Any())
            {
                return NotFound(new { message = "No films found for this category." });
            }
            return Ok(films);
        }
    }

}
