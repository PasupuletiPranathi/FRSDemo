using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public interface IRental
    {
        Task<bool> AddRentalAsync(RentalDto rentalDto);
        Task<IEnumerable<object>> GetRentedFilmsByCustomerAsync(int customerId);
        Task<IEnumerable<object>> GetTopTenMostRentedFilmsAsync();
        Task<IEnumerable<object>> GetTopTenMostRentedFilmsByStoreAsync(int storeId);
        Task<IEnumerable<object>> GetCustomersWithDueRentalsByStoreAsync(int storeId);

    }
}
