using System;
using System.Collections.Generic;

#nullable disable

namespace sss.Models
{
    public partial class Reward
    {
        public int Id { get; set; }
        public double RewardMoney { get; set; }
        public string TypeOfSuggest { get; set; }
        public bool? Status { get; set; }
    }
}
