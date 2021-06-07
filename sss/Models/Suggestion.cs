using System;
using System.Collections.Generic;

#nullable disable

namespace sss.Models
{
    public partial class Suggestion
    {
        public long Id { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
