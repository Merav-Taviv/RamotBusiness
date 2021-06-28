using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public partial class Forms
    {
        public Forms()
        {
            Files = new HashSet<Files>();
        }

        public int FormId { get; set; }
        public string FormName { get; set; }
        public DateTime LastUsing { get; set; }
        public int AmountOfUsing { get; set; }
        public int UserId { get; set; }
        public bool Sharing { get; set; }
        public string ImagePath { get; set; }

        public Users User { get; set; }
        public ICollection<Files> Files { get; set; }
    }
}
