using Flunt.Notifications;
using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystem.Core.Contexts.AccountContext.UseCases.Create
{
    public static class Specification
    {
        public static Contract<Notification> Ensure(Request request) => 
            new Contract<Notification>()
            .Requires()
            .IsLowerThan(request.Name.Length, 160, "Name", "Name should've at least 160 characters")
            .IsGreaterThan(request.Name.Length, 3, "Name", "Name should've morte than 3 characters")
            .IsLowerThan(request.Password.Length, 40, "Password", "Password should've at least 40 characters")
            .IsGreaterThan(request.Password.Length, 8, "Password", "Password should've morte than 8 characters")
            .IsEmail(request.Email, "Email", "Invalid Email");
    }
}
