using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class MessageDTO
    {
        public long MessageId { get; set; }

        public int? UserId { get; set; }

        public string? Body { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? ReadDate { get; set; }

        public int MessageTypeId { get; set; }

        public long? BookId { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool IsRead { get; set; }

        public long? ParentMessage { get; set; }

        public DateTime? DateExecution { get; set; }

        public long ConversationId { get; set; }

        public string? IpAddress { get; set; }

        public string? AdditionalData { get; set; }

        public int MessageStatusId { get; set; }
    }
}
