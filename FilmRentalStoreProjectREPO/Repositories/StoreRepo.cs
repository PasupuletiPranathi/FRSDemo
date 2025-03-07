using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FilmRentalStoreProjectDAL.Models;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class StoreRepo:IStore
    {

        private readonly FrsdbmContext _context;

        public StoreRepo(FrsdbmContext context)
        {
            _context = context;
        }

        

        public async Task<IEnumerable<Store>> GetStoresByCityAsync(string city)
        {
            return await _context.Stores
        .Include(s => s.Address)   // Join with Address Table
        .ThenInclude(a => a.City)  // Join with City Table
        .Where(s => s.Address.City.City1 == city) // Filter by City Name
        .ToListAsync();
        }

        public async Task<IEnumerable<object>> GetStoresByCountryAsync(string country)
        {
            var stores = await _context.Stores
                .Include(s => s.Address) // Join Store -> Address
                .ThenInclude(a => a.City) // Join Address -> City
                .ThenInclude(c => c.Country) // Join City -> Country
                .Where(s => s.Address.City.Country.Country1 == country) // Filter by Country Name
                .Select(s => new
                {
                    StoreId = s.StoreId,
                    Address = s.Address.Address1,
                    City = s.Address.City.City1,
                    Country = s.Address.City.Country.Country1
                })
                .ToListAsync();

            return stores;
        }

        public async Task<object> GetStoreByPhoneAsync(string phone)
        {
            var store = await _context.Stores
                .Include(s => s.Address) // Join Store -> Address
                .ThenInclude(a => a.City) // Join Address -> City
                .ThenInclude(c => c.Country) // Join City -> Country
                .Where(s => s.Address.Phone == phone) // Filter by Phone Number
                .Select(s => new
                {
                    StoreId = s.StoreId,
                    Phone = s.Address.Phone,
                    Address = s.Address.Address1,
                    City = s.Address.City.City1,
                    Country = s.Address.City.Country.Country1
                })
                .FirstOrDefaultAsync();

            return store;
        }

        public async Task<IEnumerable<object>> GetAllStaffByStoreAsync(int storeId)
        {
            var staffList = await _context.Staff
                .Where(s => s.StoreId == storeId) // Filter by Store ID
                .Select(s => new
                {
                    StaffId = s.StaffId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Active = s.Active
                })
                .ToListAsync();

            return staffList;
        }

        public async Task<IEnumerable<object>> GetAllCustomersByStoreAsync(int storeId)
        {
            var customers = await _context.Customers
                .Where(c => c.StoreId == storeId) // Filter by Store ID
                .Select(c => new
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Active = c.Active
                })
                .ToListAsync();

            return customers;
        }

        public async Task<object> GetStoreManagerAsync(int storeId)
        {
            var manager = await _context.Stores
                .Include(s => s.ManagerStaff) // Join Store -> Manager (Staff)
                .Where(s => s.StoreId == storeId) // Filter by Store ID
                .Select(s => new
                {
                    ManagerId = s.ManagerStaff.StaffId,
                    FirstName = s.ManagerStaff.FirstName,
                    LastName = s.ManagerStaff.LastName,
                    Email = s.ManagerStaff.Email,
                    Phone = s.ManagerStaff.Address.Phone,
                    Active = s.ManagerStaff.Active
                })
                .FirstOrDefaultAsync();

            return manager;
        }

        public async Task<IEnumerable<object>> GetAllStoreManagersAsync()
        {
            var managers = await _context.Stores
                .Include(s => s.ManagerStaff) // Join Store -> Manager (Staff)
                .Include(s => s.Address) // Join Store -> Address
                .ThenInclude(a => a.City) // Join Address -> City
                .Select(s => new
                {
                    ManagerFirstName = s.ManagerStaff.FirstName,
                    ManagerLastName = s.ManagerStaff.LastName,
                    ManagerEmail = s.ManagerStaff.Email,
                    ManagerPhone = s.ManagerStaff.Address.Phone,
                    StoreAddress = s.Address.Address1,
                    StoreCity = s.Address.City.City1,
                    StorePhone = s.Address.Phone
                })
                .ToListAsync();

            return managers;
        }

        








    }

}
