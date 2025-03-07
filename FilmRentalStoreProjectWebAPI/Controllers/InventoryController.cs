using FilmRentalStoreProjectREPO.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmRentalStoreProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventory _inventoryRepo;

        public InventoryController(IInventory inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;
        }

        [HttpGet("inventory")]
       
        public async Task<IActionResult> GetInventoryCount([FromQuery] int? storeId)
        {
            var inventory = await _inventoryRepo.GetInventoryCountAsync(storeId);
            if (inventory == null || !inventory.Any())
            {
                return NotFound(new { message = "No inventory data found!" });
            }

            return Ok(inventory);
        }

        [HttpGet("store/{id}")]
        public async Task<IActionResult> GetFilmInventoryByStore(int id)
        {
            var inventory = await _inventoryRepo.GetFilmInventoryByStoreAsync(id);
            if (inventory == null || !inventory.Any())
            {
                return NotFound(new { message = "No inventory data found for the given store!" });
            }

            return Ok(inventory);
        }

        [HttpGet("film/{filmId}/store/{storeId}")]
        public async Task<IActionResult> GetInventoryByFilmAndStore(int filmId, int storeId)
        {
            var inventory = await _inventoryRepo.GetInventoryByFilmAndStoreAsync(filmId, storeId);
            if (inventory == null)
            {
                return NotFound(new { message = "No inventory data found for the specified film and store!" });
            }

            return Ok(inventory);
        }

        [HttpGet("film/{filmId}")]
        public async Task<IActionResult> GetInventoryByFilm(int filmId)
        {
            var inventory = await _inventoryRepo.GetInventoryByFilmAsync(filmId);
            if (inventory == null || !inventory.Any())
            {
                return NotFound(new { message = "No inventory data found for the specified film!" });
            }

            return Ok(inventory);
        }

    }
}
