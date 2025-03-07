using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmRentalStoreProjectDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class CustomerRepo:ICustomer
    {
        private readonly FrsdbmContext _context;

        public CustomerRepo(FrsdbmContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<object>> GetActiveCustomersAsync()
        {
            var activeCustomers = await _context.Customers
                .Where(c => c.Active.ToLower() == "true" || c.Active == "1") // Handle string values
                .Select(c => new
                {
                    CustomerId = c.CustomerId,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    Active = c.Active
                })
                .ToListAsync();

            return activeCustomers;
        }


    }
}
