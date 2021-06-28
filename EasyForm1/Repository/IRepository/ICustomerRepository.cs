using Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.IRepository
{
    public interface ICustomerRepository
    {
        bool AddCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(int customerId);
    }
}
