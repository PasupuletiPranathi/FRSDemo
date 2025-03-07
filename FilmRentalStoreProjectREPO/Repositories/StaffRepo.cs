using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStoreProjectDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class StaffRepo:IStaff
    {
        private readonly FrsdbmContext _context;

        public StaffRepo(FrsdbmContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<object>> GetStaffByLastNameAsync(string lastName)
        {
            var staffList = await _context.Staff
                .Where(s => s.LastName == lastName) // Filter by Last Name
                .Select(s => new
                {
                    StaffId = s.StaffId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Phone = s.Address.Phone,
                    Active = s.Active
                })
                .ToListAsync();

            return staffList;
        }

        public async Task<IEnumerable<object>> GetStaffByFirstNameAsync(string firstName)
        {
            var staffList = await _context.Staff
                .Where(s => s.FirstName == firstName) // Filter by First Name
                .Select(s => new
                {
                    StaffId = s.StaffId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Phone = s.Address.Phone,
                    Active = s.Active
                })
                .ToListAsync();

            return staffList;
        }

        public async Task<object> GetStaffByEmailAsync(string email)
        {
            var staff = await _context.Staff
                .Where(s => s.Email == email) // Filter by Email
                .Select(s => new
                {
                    StaffId = s.StaffId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Phone = s.Address.Phone,
                    Active = s.Active
                })
                .FirstOrDefaultAsync(); // Since Email is unique, return only one record

            return staff;
        }

        public async Task<IEnumerable<object>> GetStaffByCityAsync(string city)
        {
            var staffList = await _context.Staff
                .Include(s => s.Address) // Join Staff -> Address
                .ThenInclude(a => a.City) // Join Address -> City
                .Where(s => s.Address.City.City1 == city) // Filter by City Name
                .Select(s => new
                {
                    StaffId = s.StaffId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Phone = s.Address.Phone,
                    Active = s.Active,
                    Address = s.Address.Address1,
                    City = s.Address.City.City1,
                    Country = s.Address.City.Country.Country1
                })
                .ToListAsync();

            return staffList;
        }

        public async Task<object> GetStaffByPhoneAsync(string phone)
        {
            var staff = await _context.Staff
                .Include(s => s.Address) // Join Staff -> Address
                .ThenInclude(a => a.City) // Join Address -> City
                .ThenInclude(c => c.Country) // Join City -> Country
                .Where(s => s.Address.Phone == phone) // Filter by Phone Number
                .Select(s => new
                {
                    StaffId = s.StaffId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Active = s.Active,
                    Phone = s.Address.Phone,
                    Address = s.Address.Address1,
                    City = s.Address.City.City1,
                    Country = s.Address.City.Country.Country1
                })
                .FirstOrDefaultAsync(); // Since Phone is unique, return only one record

            return staff;
        }

        public async Task<IEnumerable<object>> GetStaffByCountryAsync(string country)
        {
            var staffList = await _context.Staff
                .Include(s => s.Address) // Join Staff -> Address
                .ThenInclude(a => a.City) // Join Address -> City
                .ThenInclude(c => c.Country) // Join City -> Country
                .Where(s => s.Address.City.Country.Country1 == country) // Filter by Country Name
                .Select(s => new
                {
                    StaffId = s.StaffId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Active = s.Active,
                    Address = s.Address.Address1,
                    City = s.Address.City.City1,
                    Country = s.Address.City.Country.Country1
                })
                .ToListAsync();

            return staffList;
        }

        public async Task<bool> AddStaffAsync(StaffDto staffDto)
        {
            // Ensure the store and address exist
            var storeExists = await _context.Stores.AnyAsync(s => s.StoreId == staffDto.StoreId);
            var addressExists = await _context.Addresses.AnyAsync(a => a.AddressId == staffDto.AddressId);

            if (!storeExists || !addressExists)
            {
                return false; // Invalid Store ID or Address ID
            }

            var newStaff = new Staff
            {
                FirstName = staffDto.FirstName,
                LastName = staffDto.LastName,
                Email = staffDto.Email,
                StoreId = staffDto.StoreId,
                Active = staffDto.Active, // ✅ Now correctly using bool
                AddressId = staffDto.AddressId,
                Username = staffDto.Username,
                Password = staffDto.Password, // Consider encrypting before saving in production
                LastUpdate = DateTime.UtcNow
            };

            _context.Staff.Add(newStaff);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<object> AssignAddressToStaffAsync(int staffId, int addressId)
        {
            // Find the staff member
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId);
            if (staff == null)
            {
                return null; // Return null if staff does not exist
            }

            // Find the address with city and country included
            var address = await _context.Addresses
                .Include(a => a.City)
                .ThenInclude(c => c.Country)
                .FirstOrDefaultAsync(a => a.AddressId == addressId);

            if (address == null)
            {
                return null; // Return null if address does not exist
            }

            // Assign new address
            staff.AddressId = addressId;
            staff.LastUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new
            {
                StaffId = staff.StaffId,
                FirstName = staff.FirstName,
                LastName = staff.LastName,
                Email = staff.Email,
                AddressId = address.AddressId,
                Address = address.Address1,
                City = address.City?.City1 ?? "Unknown City",  // Handle potential null reference
                Country = address.City?.Country?.Country1 ?? "Unknown Country"  // Handle potential null reference
            };
        }









    }
}
