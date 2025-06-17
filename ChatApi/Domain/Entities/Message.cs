using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int SenderId { get; set; }
        public int? ReceiverId { get; set; } // Null nếu gửi nhóm
        public int? GroupId { get; set; } // Null nếu gửi riêng
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public Group Group { get; set; }
    }
}