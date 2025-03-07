using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStoreProjectDAL.Models;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public interface IUser
    {
        Task<Role> GetRoleByNameAsync(string roleName);
        Task<Usertable> GetUserByUsernameAndPasswordAsync(string username, string password);
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<string> GetStaffEmailByUsernameAsync(string username);
        Task<Staff> GetStaffByUsernameAsync(string username);
    }
}
