using System;
using System.Collections.Generic;

#nullable disable

namespace sss.Models
{
    public partial class Systemuser
    {
        public Systemuser()
        {
            Suggestions = new HashSet<Suggestion>();
        }

        public string Userid { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Suggestion> Suggestions { get; set; }
    }
}
