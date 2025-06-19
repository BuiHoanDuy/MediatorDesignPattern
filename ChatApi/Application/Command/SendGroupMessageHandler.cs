using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Application.Requests;
using ChatApi.Domain.Entities;
using ChatApi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Application.Handlers
{
    public class SendGroupMessageHandler : IRequestHandler<SendGroupMessageRequest, bool>
    {
        private readonly ChatDbContext _context;

    public SendGroupMessageHandler(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(SendGroupMessageRequest request, CancellationToken cancellationToken)
    {
        var sender = await _context.Users.FirstOrDefaultAsync(u => u.Name == request.SenderName, cancellationToken);
        if (sender == null) return false;

        var message = new Message
        {
            Content = request.Message,
            Timestamp = DateTime.UtcNow,
            SenderId = sender.Id,
            GroupId = request.GroupId
        };
        _context.Messages.Add(message);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
    }
}