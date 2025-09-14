using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests
{
    public class ConversationFilterDTO
    {
        public List<string> Keywords { get; set; } = new List<string>();
        public bool? UnreadOnly { get; set; }
        //public bool? OpenOnly { get; set; }
        public int? UserId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? statusId { get; set; }
    }
}
