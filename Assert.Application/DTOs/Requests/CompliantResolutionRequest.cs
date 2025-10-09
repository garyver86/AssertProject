using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests
{
    public class CompliantResolutionRequest
    {
        public string Notes { get; set; }
        public string internalNotes { get; set; }
        public string ResolutionType { get; set; }
    }
}
