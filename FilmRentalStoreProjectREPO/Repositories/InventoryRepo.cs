using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FilmRentalStoreProjectDAL.Models;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class InventoryRepo : IInventory
    {
        private readonly FrsdbmContext _context;

        public InventoryRepo(FrsdbmContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> GetInventoryCountAsync(int? storeId = null)
        {
            var query = _context.Inventories
                .Include(i => i.Film) // Join with Film table
                .AsQueryable();

            if (storeId.HasValue)
            {
                query = query.Where(i => i.StoreId == storeId);
            }

            return await query
                .GroupBy(i => i.Film.Title) // Group by Film Title
                .Select(g => new
                {
                    FilmTitle = g.Key,
                    InventoryCount = g.Count()
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<dynamic>> GetFilmInventoryByStoreAsync(int storeId)
        {
            // Query Inventory and group by Film Title
            var inventoryData = await (from inventory in _context.Inventories
                                       join film in _context.Films on inventory.FilmId equals film.FilmId
                                       where inventory.StoreId == storeId
                                       group inventory by film.Title into filmGroup
                                       select new
                                       {
                                           FilmTitle = filmGroup.Key,
                                           NumberOfCopies = filmGroup.Count() // ✅ Count the number of records instead of summing
                                       })
                                       .ToListAsync();

            return inventoryData;
        }

        public async Task<object> GetInventoryByFilmAndStoreAsync(int filmId, int storeId)
        {
            var inventoryData = await _context.Inventories
                .Include(i => i.Store) // Join with Store table
                .Include(i => i.Film) // Join with Film table
                .Where(i => i.FilmId == filmId && i.StoreId == storeId) // Filter by film and store
                .GroupBy(i => new { i.Store.Address.Address1, i.Store.Address.City.City1 }) // Group by store address
                .Select(g => new
                {
                    StoreAddress = $"{g.Key.Address1}, {g.Key.City1}", // Format store address
                    NumberOfCopies = g.Count() // Count the number of copies
                })
                .FirstOrDefaultAsync(); // Return first match (or null if none)

            return inventoryData;
        }

        public async Task<IEnumerable<object>> GetInventoryByFilmAsync(int filmId)
        {
            var inventoryData = await _context.Inventories
                .Include(i => i.Store) // Join with Store table
                .ThenInclude(s => s.Address) // Include Address details
                .Where(i => i.FilmId == filmId) // Filter by film
                .GroupBy(i => new { i.Store.Address.Address1, i.Store.Address.City.City1 }) // Group by store address
                .Select(g => new
                {
                    StoreAddress = $"{g.Key.Address1}, {g.Key.City1}", // Store Address (Formatted)
                    NumberOfCopies = g.Count() // Count of available copies
                })
                .ToListAsync();

            return inventoryData;
        }



    }

}
