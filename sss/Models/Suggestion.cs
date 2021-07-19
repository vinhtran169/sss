using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace sss.Models
{
    public partial class Suggestion
    {
        public int Id { get; set; }
        [DisplayName("Title")]
        public string Title { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Implement Date")]
        public DateTime? ImplementDate { get; set; }
        public string StatusType { get; set; }
        [DisplayName("Comment")]
        public string RemarkFromApprover { get; set; }
        [DisplayName("Reward Money")]
        public double? RewardMoney { get; set; }
        [DisplayName("Manager")]
        public string Userid { get; set; }
        public string Creator { get; set; }
        [DisplayName("Created Date")]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Updated Date")]
        public DateTime? UpdatedDate { get; set; }
        public virtual Systemuser User { get; set; }
    }
}
