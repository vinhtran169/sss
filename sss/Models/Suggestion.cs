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
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime? ImplementDate { get; set; }
        public string StatusType { get; set; }
        public string RemarkFromApprover { get; set; }
        public double? RewardMoney { get; set; }
        public string Userid { get; set; }
        public string Creator { get; set; }
        [DisplayName("Create date")]
        [Required]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Update date")]
        [Required]
        public DateTime? UpdatedDate { get; set; }

        public virtual Systemuser User { get; set; }
    }
}
