using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ChatApi.Application.Requests
{
    public class SendGroupMessageRequest : IRequest<bool>
    {
        public string SenderName { get; set; }
        public string Message { get; set; }
        public int GroupId { get; set; }
    }
}