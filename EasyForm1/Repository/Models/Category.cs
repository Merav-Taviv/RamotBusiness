using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Repository.Models
{
    public partial class Category
    {
        public int CategoryId { get; set; }
        public int CategoryName { get; set; }
        public int ProviderId { get; set; }
    }
}
