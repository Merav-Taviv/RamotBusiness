using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.Models;

namespace Repository.Repository
{
     public  class CustomerRepository : ICustomerRepository
    {
        private RamotBusinessContext context;
        public CustomerRepository(RamotBusinessContext context)
        {
            this.context = context;
        }
        public bool AddCustomer(Customer customer
            )
        {
            if (context.Customer.Contains(customer))
            {
                return false;
            }

            context.Customer.Add(customer);
            return context.SaveChanges() > 0;
        }
        public bool UpdateCustomer(Customer customer)
        {
            Customer c = context.Customer.Where(a => a.CustomerId == customer.CustomerId).First();
            c.CustomerName = customer.CustomerName;
            c.Phone = customer.Phone;
            c.Mail = customer.Mail;
            context.Customer.Update(c);
            return context.SaveChanges()>0;
        }
        public bool DeleteCustomer(int customerId)
        {

            foreach (Customer item in context.Customer)
            {
                if (item.CustomerId == customerId)
                {
                    context.Customer.Remove(item);
                }
            }
            return context.SaveChanges() > 0;

        }
    }
}
