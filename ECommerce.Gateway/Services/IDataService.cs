using ECommerce.Gateway.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Gateway.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Customer>> GetCustomerAsync(Customer customer);
    }
}
