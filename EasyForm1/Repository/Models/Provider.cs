using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Repository.Models
{
    public partial class Provider
    {
        public Provider()
        {
            Category = new HashSet<Category>();
            Feedback = new HashSet<Feedback>();
        }

        public int ProviderId { get; set; }
        public int ProviderName { get; set; }
        public string Neighborhood { get; set; }
        public int Address { get; set; }
        public int Phone { get; set; }
        public int Mobile { get; set; }
        public int CategoryId { get; set; }
        public string Pictuer { get; set; }
        public string Mail { get; set; }
        public string TimeOpen { get; set; }

        public virtual Category CategoryNavigation { get; set; }
        public virtual ICollection<Category> Category { get; set; }
        public virtual ICollection<Feedback> Feedback { get; set; }
    }
}
