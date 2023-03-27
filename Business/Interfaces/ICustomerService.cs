using Business.Models;
using Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICustomerService : ICrud<CustomerModel>
    {
        Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId);
        CustomerModel mappToCustomerModel(CreateCustomerModel model);
        Customer mappToCustomer(CreateCustomerModel model);
    }
}
