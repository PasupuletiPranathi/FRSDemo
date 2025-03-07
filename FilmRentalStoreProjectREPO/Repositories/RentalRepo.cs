using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStoreProjectDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class RentalRepo:IRental
    {
        private readonly FrsdbmContext _context;

        public RentalRepo(FrsdbmContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<object>> GetTopTenMostRentedFilmsAsync()
        {
            var topFilms = await _context.Rentals
                .Include(r => r.Inventory)
                .ThenInclude(i => i.Film) // Join Rentals -> Inventory -> Film
                .GroupBy(r => r.Inventory.Film.Title) // Group by Film Title
                .Select(g => new
                {
                    FilmTitle = g.Key,
                    RentalCount = g.Count() // Count total rentals for each film
                })
                .OrderByDescending(f => f.RentalCount) // Sort by highest rentals
                .Take(10) // Get only top 10 films
                .ToListAsync();

            return topFilms;
        }

        public async Task<IEnumerable<object>> GetRentedFilmsByCustomerAsync(int customerId)
        {
            var rentedFilms = await _context.Rentals
                .Include(r => r.Inventory)
                .ThenInclude(i => i.Film) // Join Rentals → Inventory → Film
                .Where(r => r.CustomerId == customerId) // Filter by customer
                .Select(r => new
                {
                    FilmTitle = r.Inventory.Film.Title,
                    RentalDate = r.RentalDate
                })
                .ToListAsync();

            return rentedFilms;
        }


        public async Task<IEnumerable<object>> GetTopTenMostRentedFilmsByStoreAsync(int storeId)
        {
            var topFilms = await _context.Rentals
                .Include(r => r.Inventory)
                .ThenInclude(i => i.Film) // Join Rentals -> Inventory -> Film
                .Where(r => r.Inventory.StoreId == storeId) // Filter by store
                .GroupBy(r => r.Inventory.Film.Title) // Group by Film Title
                .Select(g => new
                {
                    FilmTitle = g.Key,
                    RentalCount = g.Count() // Count total rentals for each film
                })
                .OrderByDescending(f => f.RentalCount) // Sort by highest rentals
                .Take(10) // Get only top 10 films
                .ToListAsync();

            return topFilms;
        }

        public async Task<IEnumerable<object>> GetCustomersWithDueRentalsByStoreAsync(int storeId)
        {
            var dueCustomers = await _context.Rentals
                .Include(r => r.Customer) // Join Rentals -> Customer
                .Include(r => r.Inventory)
                .ThenInclude(i => i.Store) // Join Rentals -> Inventory -> Store
                .Where(r => r.Inventory.StoreId == storeId && r.ReturnDate == null) // Filter by store & due rentals
                .Select(r => new
                {
                    CustomerId = r.Customer.CustomerId,
                    FirstName = r.Customer.FirstName,
                    LastName = r.Customer.LastName,
                    Email = r.Customer.Email,
                    RentalDate = r.RentalDate
                })
                .Distinct() // Ensure unique customers
                .ToListAsync();

            return dueCustomers;
        }

        public async Task<bool> AddRentalAsync(RentalDto rentalDto)
        {
            // Find available inventory for the given film in the specified store
            var inventoryItem = await _context.Inventories
                .FirstOrDefaultAsync(i => i.FilmId == rentalDto.FilmId && i.StoreId == rentalDto.StoreId);

            if (inventoryItem == null)
            {
                return false; // No available film in the store
            }

            var rental = new Rental
            {
                CustomerId = rentalDto.CustomerId,
                InventoryId = inventoryItem.InventoryId,
                RentalDate = rentalDto.RentalDate,
                ReturnDate = null, // Initially null (not returned)
                StaffId = 1, // Assign to a default staff member (modify if needed)
                LastUpdate = DateTime.UtcNow
            };

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
