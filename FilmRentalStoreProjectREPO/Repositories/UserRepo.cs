using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStoreProjectDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class UserRepo :IUser
    {
        private readonly FrsdbmContext _context;

        public UserRepo(FrsdbmContext context)
        {
            _context = context;
        }

        public async Task<Usertable> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            return await _context.Usertables.Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FindAsync(roleId);
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _context.Roles
            .FirstOrDefaultAsync(r => r.RoleName.Equals(roleName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<string> GetStaffEmailByUsernameAsync(string username)
        {
            var staff = await _context.Staff
                .FirstOrDefaultAsync(s => s.FirstName == username); // ✅ Match username with staff table

            return staff?.Email; // ✅ Return email if found, otherwise null
        }
        public async Task<Staff> GetStaffByUsernameAsync(string username)
        {
            return await _context.Staff.FirstOrDefaultAsync(s => s.Username == username);
        }

    }
}
