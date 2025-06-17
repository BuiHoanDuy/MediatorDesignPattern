using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatApi.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<GroupUser> GroupUsers { get; set; }
    }
}