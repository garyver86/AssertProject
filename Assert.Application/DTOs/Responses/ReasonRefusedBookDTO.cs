using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class ReasonRefusedBookDTO
    {
        public int ReasonRefusedId { get; set; }

        public string ReasonRefusedCode { get; set; } = null!;

        public string ReasonRefusedName { get; set; } = null!;

        public string? ReasonRefusedText { get; set; }
    }
    public class ReasonRefusedConsultDTO
    {
        public int ReasonRefusedId { get; set; }

        public string ReasonRefusedCode { get; set; } = null!;

        public string ReasonRefusedName { get; set; } = null!;

        public string? ReasonRefusedText { get; set; }
    }
}
