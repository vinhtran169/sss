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
        [Required(ErrorMessage = "Please fill title for this suggestion")]
        [MinLength(10, ErrorMessage = "Please lengthen title to 10 characters or more")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please fill description for this suggestion")]
        [MinLength(10, ErrorMessage = "Please lengthen description to 10 characters or more")]
        public string Description { get; set; }
        [DisplayName("Implement date")]
        [Required(ErrorMessage = "Please fill implementDate date for this suggestion")]
        [DataType(DataType.Date)]
        public DateTime? ImplementDate { get; set; }
        public string StatusType { get; set; }
        public string RemarkFromApprover { get; set; }
        public double? RewardMoney { get; set; }
        public string Userid { get; set; }
        public string Creator { get; set; }
        [DisplayName("Create date")]
        [Required(ErrorMessage = "Please fill created date for this suggestion")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        [DisplayName("Update date")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { get; set; }
        public virtual Systemuser User { get; set; }
    }
}
