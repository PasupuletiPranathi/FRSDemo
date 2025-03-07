using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStoreProjectDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class FilmRepo:IFilm
    {

            private readonly FrsdbmContext _context;

            public FilmRepo(FrsdbmContext context)

            {

                _context = context;

            }
            public async Task<Film> AddFilmAsync(Film film)

            {

                if (film == null)

                {

                    throw new ArgumentNullException(nameof(film));

                }

                await _context.Films.AddAsync(film);

                await _context.SaveChangesAsync();

                return film;

            }

            public async Task<IEnumerable<Film>> GetFilmsByTitleAsync(string title)

            {

                return await _context.Films.Where(f => f.Title.Contains(title)).ToListAsync();

            }

            public async Task<IEnumerable<Film>> GetFilmsByYear(int year)

            {

                return await _context.Films.Where(f => f.ReleaseYear == year.ToString()).ToListAsync();

            }

            public async Task<IEnumerable<Film>> GetFilmsByRentalDurationGreater(int duration)

            {

                return await _context.Films.Where(f => f.RentalDuration > duration).ToListAsync();

            }

            public async Task<IEnumerable<Film>> GetFilmsByRentalRateGreater(decimal rate)

            {

                return await _context.Films.Where(f => f.RentalRate > rate).ToListAsync();

            }

            public async Task<IEnumerable<Film>> GetFilmsByLengthGreater(int length)

            {

                return await _context.Films.Where(f => f.Length > length).ToListAsync();

            }

            public async Task<IEnumerable<Film>> GetFilmsByRentalDurationLower(int duration)

            {

                return await _context.Films.Where(f => f.RentalDuration < duration).ToListAsync();

            }

            public async Task<IEnumerable<Film>> GetFilmsByRentalRateLower(decimal rate)

            {

                return await _context.Films.Where(f => f.RentalRate < rate).ToListAsync();

            }

            public async Task<IEnumerable<Film>> GetFilmsByLength(int length)

            {

                return await _context.Films

                    .Where(f => f.Length < length)

                    .ToListAsync();

            }

            public async Task<IEnumerable<Film>> GetFilmsByYearRange(int from, int to)

            {

                return (await _context.Films

            .Where(f => f.ReleaseYear != null)

            .ToListAsync())

            .Where(f => int.TryParse(f.ReleaseYear, out int releaseYear)
            && releaseYear >= from
            && releaseYear <= to);

            }

            public async Task<List<Film>> GetFilmsByRatingLowerThan(string rating)

            {

                var films = await _context.Films.ToListAsync(); // Fetch all films

                return films

                    .Where(f => f.Rating != null && string.Compare(f.Rating, rating, StringComparison.Ordinal) < 0)

                    .ToList();

            }

            public async Task<List<Film>> GetFilmsByRatingGreaterThan(string rating)

            {

                var films = await _context.Films.ToListAsync(); // Fetch all films

                return films

                    .Where(f => f.Rating != null && string.Compare(f.Rating, rating, StringComparison.Ordinal) > 0)

                    .ToList();

            }

            public async Task<IEnumerable<Film>> GetFilmsByLanguage(string lang)

            {

                return await _context.Films

            .Where(f => f.Language.Name == lang)

            .ToListAsync();

            }


            public async Task<IEnumerable<KeyValuePair<int, int>>> GetFilmCountByYearAsync()

            {

                return await _context.Films

            .GroupBy(f => Convert.ToInt32(f.ReleaseYear))

            .Select(g => new KeyValuePair<int, int>(g.Key, g.Count()))

            .ToListAsync();

            }

            public async Task<IEnumerable<Actor>> GetActorsByFilmIdAsync(int filmId)

            {

                return await _context.Actors

                    .Where(a => _context.FilmActors

                        .Any(fa => fa.FilmId == filmId && fa.ActorId == a.ActorId))

                    .ToListAsync();

            }

            public async Task<IEnumerable<Film>> GetFilmsByCategoryAsync(string category)

            {

                return await _context.Films

            .Where(f => f.FilmCategories.Any(fc => fc.Category.Name == category))

            .ToListAsync();

            }

        }
    }
