using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Domain.Entities;
using ChatApi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Application.Requests
{
    public class CreateGroupHandler : IRequestHandler<CreateGroupRequest, bool>
    {
        private readonly ChatDbContext _context;

        public CreateGroupHandler(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateGroupRequest request, CancellationToken cancellationToken)
        {
            var group = new Group { Name = request.GroupName };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync(cancellationToken);

            var users = await _context.Users
                .Where(u => request.UserNames.Contains(u.Name))
                .ToListAsync(cancellationToken);

            var groupUsers = users.Select(u => new GroupUser { UserId = u.Id, GroupId = group.Id }).ToList();
            _context.GroupUsers.AddRange(groupUsers);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}