using LoginSystem.Core.AccountContext.ValueObjects;
using LoginSystem.Core.Contexts.AccountContext.ValueObjects;
using LoginSystem.Core.Contexts.SharedContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystem.Core.Contexts.AccountContext.Entities
{
    public class User : Entity
    {
        protected User() { }

        public User(string name, Email email, Password password)
        {
            Email = email;
            Password = password;
            Name = name;
        }

        public User(string email, string? password = null)
        {
            Email = email;
            Password = new(password);
        }
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public string Image { get; private set; }
        public void UpdatePassword(string plainTextPassword, string code)
        {
            if (!string.Equals(code.Trim(), Password.ResetCode.Trim(), StringComparison.CurrentCultureIgnoreCase))
                throw new Exception("Invalid restore token.");

            var password = new Password(plainTextPassword);
            Password = password;
        }

        public void UpdateEmail(Email email)
        {
            Email = email;
        }

        public void ChangePassword(string plainTextPassword)
        {
            var password = new Password(plainTextPassword);
            Password = password;
        }
    }
}
