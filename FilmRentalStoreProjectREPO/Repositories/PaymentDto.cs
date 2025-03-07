using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class PaymentDto
    {
        public int CustomerId { get; set; }
        public int StaffId { get; set; }
        public int RentalId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
