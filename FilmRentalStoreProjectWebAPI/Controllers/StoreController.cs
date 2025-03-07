using FilmRentalStoreProjectREPO.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmRentalStoreProjectWebAPI.Controllers
{
    
    [ApiController]
    [Route("api/store")]
    public class StoreController : ControllerBase
    {
        private readonly IStore _storeRepo;

        public StoreController(IStore storeRepo)
        {
            _storeRepo = storeRepo;
        }

        




        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetStoresByCity(string city)
        {
            var stores = await _storeRepo.GetStoresByCityAsync(city);
            return stores.Any() ? Ok(stores) : NotFound("No stores found in the specified city.");
        }


        [HttpGet("country/{country}")]
        public async Task<IActionResult> GetStoresByCountry(string country)
        {
            var stores = await _storeRepo.GetStoresByCountryAsync(country);
            if (stores == null || !stores.Any())
            {
                return NotFound(new { message = "No stores found in the specified country!" });
            }

            return Ok(stores);
        }

        [HttpGet("phone/{phone}")]
        public async Task<IActionResult> GetStoreByPhone(string phone)
        {
            var store = await _storeRepo.GetStoreByPhoneAsync(phone);
            if (store == null)
            {
                return NotFound(new { message = "No store found with the specified phone number!" });
            }

            return Ok(store);
        }

        [HttpGet("staff/{storeId}")]
        public async Task<IActionResult> GetAllStaffByStore(int storeId)
        {
            var staff = await _storeRepo.GetAllStaffByStoreAsync(storeId);
            if (staff == null || !staff.Any())
            {
                return NotFound(new { message = "No staff found for the specified store!" });
            }

            return Ok(staff);
        }

        [HttpGet("customer/{storeId}")]
        public async Task<IActionResult> GetAllCustomersByStore(int storeId)
        {
            var customers = await _storeRepo.GetAllCustomersByStoreAsync(storeId);
            if (customers == null || !customers.Any())
            {
                return NotFound(new { message = "No customers found for the specified store!" });
            }

            return Ok(customers);
        }

        [HttpGet("manager/{storeId}")]
        public async Task<IActionResult> GetStoreManager(int storeId)
        {
            var manager = await _storeRepo.GetStoreManagerAsync(storeId);
            if (manager == null)
            {
                return NotFound(new { message = "No manager found for the specified store!" });
            }

            return Ok(manager);
        }

        [HttpGet("managers")]
        public async Task<IActionResult> GetAllStoreManagers()
        {
            var managers = await _storeRepo.GetAllStoreManagersAsync();
            if (managers == null || !managers.Any())
            {
                return NotFound(new { message = "No managers found for any store!" });
            }

            return Ok(managers);
        }







    }
}
