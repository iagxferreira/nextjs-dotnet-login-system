using LoginSystem.Core.Contexts.SharedContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystem.Core.Contexts.AccountContext.Entities
{
    public class Role : Entity
    {
        public string Name;
        public List<User> Users { get; set; } = new();
    }
}
