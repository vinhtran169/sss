using System;
using System.Collections.Generic;

#nullable disable

namespace sss.Models
{
    public partial class Account
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }
}
