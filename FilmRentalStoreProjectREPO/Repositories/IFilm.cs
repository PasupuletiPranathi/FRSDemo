using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStoreProjectDAL.Models;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public interface IFilm
    {
        Task<Film> AddFilmAsync(Film film);
        Task<IEnumerable<Film>> GetFilmsByTitleAsync(string title);
        Task<IEnumerable<Film>> GetFilmsByYear(int year);
        Task<IEnumerable<Film>> GetFilmsByRentalDurationGreater(int duration);
        Task<IEnumerable<Film>> GetFilmsByRentalRateGreater(decimal rate);
        Task<IEnumerable<Film>> GetFilmsByLengthGreater(int length);
        Task<IEnumerable<Film>> GetFilmsByRentalDurationLower(int duration);
        Task<IEnumerable<Film>> GetFilmsByRentalRateLower(decimal rate);
        Task<IEnumerable<Film>> GetFilmsByLength(int length);
        Task<IEnumerable<Film>> GetFilmsByYearRange(int from, int to);
        Task<List<Film>> GetFilmsByRatingLowerThan(string rating);
        Task<List<Film>> GetFilmsByRatingGreaterThan(string rating);
        Task<IEnumerable<Film>> GetFilmsByLanguage(string lang);
        Task<IEnumerable<Actor>> GetActorsByFilmIdAsync(int filmId);
        Task<IEnumerable<Film>> GetFilmsByCategoryAsync(string category);
        Task<IEnumerable<KeyValuePair<int, int>>> GetFilmCountByYearAsync();
    }
}
