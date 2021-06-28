using System;
using System.Collections.Generic;

namespace Repository.Models
{
    public partial class Users
    {
        public Users()
        {
            Forms = new HashSet<Forms>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Forms> Forms { get; set; }
    }
}
