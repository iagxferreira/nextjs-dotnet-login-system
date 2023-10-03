using LoginSystem.Core.Contexts.AccountContext.Entities;
using LoginSystem.Core.Contexts.AccountContext.UseCases.Authenticate.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystem.Core.Contexts.AccountContext.UseCases.Authenticate
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IRepository _repository;
        public Handler(IRepository repository) => _repository = repository;

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            #region Validate request
            try
            {
                var res = Specification.Ensure(request);
                if (!res.IsValid)
                {
                    return new Response("Invalid request", 400, res.Notifications);
                }

            }
            catch (Exception)
            {

                return new Response("Improcessable request!", 500);
            }
            #endregion

            #region Restore profile
            User? user;
            try
            {
                user = await _repository.GetUserByEmailAsync(request.Email, cancellationToken);
                if (user is null) return new Response("Profile not found", 404);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            #endregion

            #region Check password
            if (!user.Password.Challenge(request.Password)) return new Response("Invalid user or password!", 400);
            #endregion

            #region Check if account is active
            try
            {
                if (!user.Email.Verification.IsActive) return new Response("Inactive account", 400);
            }
            catch
            {
                return new Response("We have failed while trying to verify your profile", 500);
            }
            #endregion

            #region Return data
            try
            {
                var data = new ResponseData
                {
                    Id = user.Id.ToString(),
                    Name = user.Name,
                    Email = user.Email,
                    Roles = user.Roles.Select(x => x.Name).ToArray(),
                };

                return new Response(string.Empty, data);
            }
            catch (Exception)
            {
                return new Response("We have failed while trying to get your profile", 500);
            }
            #endregion

        }
    }
}
