using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ChatApi.Application.Requests
{
    public class SendMessageRequest : IRequest<bool>
    {
        public string SenderName { get; set; }
        public string Message { get; set; }
        public string ReceiverName { get; set; } // Có thể null nếu gửi tất cả
    }
}