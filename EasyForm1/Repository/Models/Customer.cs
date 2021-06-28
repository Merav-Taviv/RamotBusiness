using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Repository.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Feedback = new HashSet<Feedback>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int Phone { get; set; }
        public string Mail { get; set; }

        public virtual ICollection<Feedback> Feedback { get; set; }
    }
}
