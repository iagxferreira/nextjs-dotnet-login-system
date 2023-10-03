using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystem.Core.Contexts.AccountContext.UseCases.Authenticate
{
    public static class Specification
    {
        public static Contract<Notification> Ensure(Request request) =>
            new Contract<Notification>()
            .Requires()
            .IsLowerThan(request.Password.Length, 40, "Password", "Password should've at least 40 characters")
            .IsGreaterThan(request.Password.Length, 8, "Password", "Password should've morte than 8 characters")
            .IsEmail(request.Email, "Email", "Invalid Email");
    }
}
