using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public class RentalDto
    {
        public int CustomerId { get; set; }
        public int FilmId { get; set; }
        public int StoreId { get; set; }
        public DateTime RentalDate { get; set; } = DateTime.UtcNow;
    }
}
