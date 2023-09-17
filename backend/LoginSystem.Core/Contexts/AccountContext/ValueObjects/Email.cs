using LoginSystem.Core.Contexts.AccountContext.ValueObjects;
using LoginSystem.Core.Contexts.SharedContext.Extensions;
using LoginSystem.Core.Contexts.SharedContext.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LoginSystem.Core.AccountContext.ValueObjects
{
    public partial class Email : ValueObject
    {
        protected Email() { }
        public Email(string address)
        {
            if(string.IsNullOrEmpty(address)) throw new Exception("Null email");
            
            Address = address.Trim().ToLower();

            if (Address.Length < 5) throw new Exception("Email length should be greater than 5 characters");
            if (!EmailRegex().IsMatch(Address)) throw new Exception("Invalid email format.");
        }

        private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        public string Address { get; set; }
        public string Hash => Address.ToBase64();
        public Verification Verification { get; private set; } = new();
        public void ResendVerification()
        {
            Verification = new();
        }

        public static implicit operator string(Email email) => email.ToString();
        public static implicit operator Email(string address) => new(address);

        public override string ToString() => Address;

        [GeneratedRegex(Pattern)]
        private static partial Regex EmailRegex();
    }
}
