using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public interface IStaff
    {
        Task<bool> AddStaffAsync(StaffDto staffDto);
        Task<IEnumerable<object>> GetStaffByLastNameAsync(string lastName);
        Task<IEnumerable<object>> GetStaffByFirstNameAsync(string firstName);
        Task<object> GetStaffByEmailAsync(string email);
        Task<IEnumerable<object>> GetStaffByCityAsync(string city);
        Task<object> GetStaffByPhoneAsync(string phone);
        Task<IEnumerable<object>> GetStaffByCountryAsync(string country);
        Task<object> AssignAddressToStaffAsync(int staffId, int addressId);
    }
}
