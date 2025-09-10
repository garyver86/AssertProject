using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs
{
    public class MessageStatusDTO
    {
        public int MessageStatus { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
