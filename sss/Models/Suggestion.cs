using System;
using System.Collections.Generic;

#nullable disable

namespace sss.Models
{
    public partial class Suggestion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ImplementDate { get; set; }
        public string StatusType { get; set; }
        public string RemarkFromApprover { get; set; }
        public double? RewardMoney { get; set; }
        public string Userid { get; set; }
        public string Creator { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual Systemuser User { get; set; }
    }
}
