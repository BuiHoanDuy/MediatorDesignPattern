using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace ChatApi.Application.Requests
{
    public class CreateGroupRequest : IRequest<bool>
{
    public string GroupName { get; set; }
    public List<string> UserNames { get; set; }
}
}