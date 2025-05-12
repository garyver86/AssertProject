using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class SpaceTypeDTO
    {
        public int SpaceTypeId { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Icon { get; set; }

        public bool? Status { get; set; }
    }
}
