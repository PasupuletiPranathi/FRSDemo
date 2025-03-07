using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStoreProjectDAL.Models;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public interface IStore
    {
       
        Task<IEnumerable<Store>> GetStoresByCityAsync(string city);
        Task<IEnumerable<object>> GetStoresByCountryAsync(string country);
        Task<object> GetStoreByPhoneAsync(string phone);
        Task<IEnumerable<object>> GetAllStaffByStoreAsync(int storeId);
        Task<IEnumerable<object>> GetAllCustomersByStoreAsync(int storeId);

        Task<object> GetStoreManagerAsync(int storeId);
        Task<IEnumerable<object>> GetAllStoreManagersAsync();

    }
}
