using FilmRentalStoreProjectDAL.Models;
using FilmRentalStoreProjectREPO.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStoreProjectWebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    
    public class RentalController : ControllerBase
    {
        private readonly IRental _rentalRepo;

        public RentalController(IRental rentalRepo)
        {
            _rentalRepo = rentalRepo;
        }
        [HttpGet("toptenfilms")]
        public async Task<IActionResult> GetTopTenMostRentedFilms()
        {
            var films = await _rentalRepo.GetTopTenMostRentedFilmsAsync();
            if (films == null || !films.Any())
            {
                return NotFound(new { message = "No rental data found!" });
            }

            return Ok(films);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetRentedFilmsByCustomer(int customerId)
        {
            var films = await _rentalRepo.GetRentedFilmsByCustomerAsync(customerId);
            if (films == null || !films.Any())
            {
                return NotFound(new { message = "No rental records found for the specified customer!" });
            }

            return Ok(films);
        }

        [HttpGet("toptenfilms/store/{storeId}")]
        public async Task<IActionResult> GetTopTenMostRentedFilmsByStore(int storeId)
        {
            var films = await _rentalRepo.GetTopTenMostRentedFilmsByStoreAsync(storeId);
            if (films == null || !films.Any())
            {
                return NotFound(new { message = "No rental data found for the specified store!" });
            }

            return Ok(films);
        }

        [HttpGet("due/store/{storeId}")]
        public async Task<IActionResult> GetCustomersWithDueRentalsByStore(int storeId)
        {
            var customers = await _rentalRepo.GetCustomersWithDueRentalsByStoreAsync(storeId);
            if (customers == null || !customers.Any())
            {
                return NotFound(new { message = "No customers with due rentals found for the specified store!" });
            }

            return Ok(customers);
        }

        [HttpPost("add")]
        public async Task<IActionResult> RentFilm([FromBody] RentalDto rentalDto)
        {
            if (rentalDto == null)
            {
                return BadRequest(new { message = "Invalid rental request!" });
            }

            var result = await _rentalRepo.AddRentalAsync(rentalDto);
            if (!result)
            {
                return BadRequest(new { message = "Film not available in the specified store!" });
            }

            return Ok(new { message = "Record Created Successfully" });
        }





    }
}
