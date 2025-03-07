using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmRentalStoreProjectREPO.Repositories
{
    public interface IPayment
    {
        Task<bool> AddPaymentAsync(PaymentDto paymentDto);
    }
}
