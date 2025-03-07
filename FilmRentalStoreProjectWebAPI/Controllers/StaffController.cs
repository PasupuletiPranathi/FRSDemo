using FilmRentalStoreProjectREPO.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmRentalStoreProjectWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaff _staffRepo;

        public StaffController(IStaff staffRepo)
        {
            _staffRepo = staffRepo;
        }


        [HttpGet("lastname/{lastName}")]
        public async Task<IActionResult> GetStaffByLastName(string lastName)
        {
            var staff = await _staffRepo.GetStaffByLastNameAsync(lastName);
            if (staff == null || !staff.Any())
            {
                return NotFound(new { message = "No staff found with the specified last name!" });
            }

            return Ok(staff);
        }

        [HttpGet("firstname/{firstName}")]
        public async Task<IActionResult> GetStaffByFirstName(string firstName)
        {
            var staff = await _staffRepo.GetStaffByFirstNameAsync(firstName);
            if (staff == null || !staff.Any())
            {
                return NotFound(new { message = "No staff found with the specified first name!" });
            }

            return Ok(staff);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetStaffByEmail(string email)
        {
            var staff = await _staffRepo.GetStaffByEmailAsync(email);
            if (staff == null)
            {
                return NotFound(new { message = "No staff found with the specified email!" });
            }

            return Ok(staff);
        }

        [HttpGet("city/{city}")]
        public async Task<IActionResult> GetStaffByCity(string city)
        {
            var staff = await _staffRepo.GetStaffByCityAsync(city);
            if (staff == null || !staff.Any())
            {
                return NotFound(new { message = "No staff found in the specified city!" });
            }

            return Ok(staff);
        }

        [HttpGet("phone/{phone}")]
        public async Task<IActionResult> GetStaffByPhone(string phone)
        {
            var staff = await _staffRepo.GetStaffByPhoneAsync(phone);
            if (staff == null)
            {
                return NotFound(new { message = "No staff found with the specified phone number!" });
            }

            return Ok(staff);
        }

        [HttpGet("country/{country}")]
        public async Task<IActionResult> GetStaffByCountry(string country)
        {
            var staff = await _staffRepo.GetStaffByCountryAsync(country);
            if (staff == null || !staff.Any())
            {
                return NotFound(new { message = "No staff found in the specified country!" });
            }

            return Ok(staff);
        }

        [HttpPost("post")]
        public async Task<IActionResult> AddStaff([FromBody] StaffDto staffDto)
        {
            if (staffDto == null)
            {
                return BadRequest(new { message = "Invalid staff data!" });
            }

            var result = await _staffRepo.AddStaffAsync(staffDto);
            if (!result)
            {
                return BadRequest(new { message = "Invalid Store ID or Address ID!" });
            }

            return Ok(new { message = "Record Created Successfully" });
        }


        [HttpPut("{staffId}/address")]
        public async Task<IActionResult> AssignAddressToStaff(int staffId, [FromBody] AssignAddressDto addressDto)
        {
            if (addressDto == null)
            {
                return BadRequest(new { message = "Invalid request data!" });
            }

            var updatedStaff = await _staffRepo.AssignAddressToStaffAsync(staffId, addressDto.AddressId);
            if (updatedStaff == null)
            {
                return NotFound(new { message = "Staff or Address not found!" });
            }

            return Ok(new
            {
                message = "Address Assigned Successfully",
                staffDetails = updatedStaff
            });
        }




    }
}
