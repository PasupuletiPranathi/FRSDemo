using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public interface IInventory
    {
        Task<IEnumerable<object>> GetInventoryCountAsync(int? storeId);
        Task<IEnumerable<dynamic>> GetFilmInventoryByStoreAsync(int storeId);
        Task<object> GetInventoryByFilmAndStoreAsync(int filmId, int storeId);
        Task<IEnumerable<object>> GetInventoryByFilmAsync(int filmId);
    }
}
