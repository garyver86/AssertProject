using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class ConversationDTO
    {
        public long ConversationId { get; set; }

        public int UserIdOne { get; set; }

        public int UserIdTwo { get; set; }

        public int StatusId { get; set; }

        public DateTime? CreationDate { get; set; }
        public UserDTO UserIdOneNavigation { get; set; } = null!;

        public UserDTO UserIdTwoNavigation { get; set; } = null!;
    }
}
