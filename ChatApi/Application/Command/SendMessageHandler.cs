using MediatR;
using ChatApi.Domain.Entities;
using ChatApi.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using ChatApi.Application.Requests;

namespace ChatApi.Application.Handlers;

public class SendMessageHandler : IRequestHandler<SendMessageRequest, bool>
{
    private readonly ChatDbContext _context;

    public SendMessageHandler(ChatDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(SendMessageRequest request, CancellationToken cancellationToken)
    {
        // Tìm hoặc tạo người gửi
        var sender = await _context.Users
            .FirstOrDefaultAsync(u => u.Name == request.SenderName, cancellationToken);
        if (sender == null)
        {
            sender = new User { Name = request.SenderName, ConnectionId = Guid.NewGuid().ToString() };
            _context.Users.Add(sender);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Nếu có ReceiverName, gửi tin nhắn riêng
        if (!string.IsNullOrEmpty(request.ReceiverName))
        {
            var receiver = await _context.Users
                .FirstOrDefaultAsync(u => u.Name == request.ReceiverName, cancellationToken);
            if (receiver == null)
            {
                receiver = new User { Name = request.ReceiverName, ConnectionId = Guid.NewGuid().ToString() };
                _context.Users.Add(receiver);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var message = new Message
            {
                Content = request.Message,
                Timestamp = DateTime.UtcNow,
                SenderId = sender.Id,
                ReceiverId = receiver.Id
            };
            _context.Messages.Add(message);
            await _context.SaveChangesAsync(cancellationToken);

            Console.WriteLine($"Gửi từ {request.SenderName} đến {request.ReceiverName}: {request.Message}");
        }
        else
        {
            // Gửi cho tất cả người dùng khác (trừ người gửi)
            var users = await _context.Users
                .Where(u => u.Name != request.SenderName)
                .ToListAsync(cancellationToken);

            foreach (var user in users)
            {
                var message = new Message
                {
                    Content = request.Message,
                    Timestamp = DateTime.Now,
                    SenderId = sender.Id,
                    ReceiverId = user.Id
                };
                _context.Messages.Add(message);
            }
            await _context.SaveChangesAsync(cancellationToken);

            Console.WriteLine($"Gửi từ {request.SenderName} đến tất cả: {request.Message}");
        }

        return true;
    }
}