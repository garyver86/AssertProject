using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Entities
{
    public class ComplaintReasonDto
    {
        public int ComplaintReasonId { get; set; }

        public string ComplaintReasonCode { get; set; } = null!;

        public string ReasonDescription { get; set; } = null!;

        public bool? IsActive { get; set; }

        public bool? RequiresFreeText { get; set; }

        public int? SeverityLevel { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? ParentId { get; set; }
    }

    public class ComplaintReasonHierarchyDto : ComplaintReasonDto
    {
        public int Level { get; set; }
        public string HierarchyPath { get; set; } = string.Empty;
        public string FullDescriptionPath { get; set; } = string.Empty;
    }
}
