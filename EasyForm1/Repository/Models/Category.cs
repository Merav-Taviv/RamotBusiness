using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Repository.Models
{
    public partial class Category
    {
        public Category()
        {
            ProviderNavigation = new HashSet<Provider>();
            SubCategory = new HashSet<SubCategory>();
        }

        public int CategoryId { get; set; }
        public int CategoryName { get; set; }
        public int ProviderId { get; set; }

        public virtual Provider Provider { get; set; }
        public virtual ICollection<Provider> ProviderNavigation { get; set; }
        public virtual ICollection<SubCategory> SubCategory { get; set; }
    }
}
