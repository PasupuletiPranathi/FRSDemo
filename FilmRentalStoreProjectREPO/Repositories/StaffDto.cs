using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class StaffDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int StoreId { get; set; }
        public bool Active { get; set; } // ✅ Now correctly using bool
        public int AddressId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
