using FilmRentalStoreProjectREPO.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmRentalStoreProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _customerRepo;

        public CustomerController(ICustomer customerRepo)
        {
            _customerRepo = customerRepo;
        }
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCustomers()
        {
            var customers = await _customerRepo.GetActiveCustomersAsync();
            if (customers == null || !customers.Any())
            {
                return NotFound(new { message = "No active customers found!" });
            }

            return Ok(customers);
        }

    }
}
