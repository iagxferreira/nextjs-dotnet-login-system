using LoginSystem.Core.AccountContext.ValueObjects;
using LoginSystem.Core.Contexts.AccountContext.Entities;
using LoginSystem.Core.Contexts.AccountContext.UseCases.Create.Contracts;
using LoginSystem.Core.Contexts.AccountContext.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystem.Core.Contexts.AccountContext.UseCases.Create
{
    public class Handler : IRequestHandler<Request, Response>
    {
        private readonly IRepository _repository;
        private readonly IService _service;

        public Handler(IRepository repository, IService service)
        {
            _repository = repository;
            _service = service;
        }

        public async Task<Response> Handle(Request request, CancellationToken cancellationToken) {
            
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
            #region Generate objects
            Email email;
            Password password;
            User user;

            try
            {
                email = new Email(request.Email);
                password = new Password(request.Password);
                user = new User(request.Name, email, password);
            }
            catch (Exception ex)
            {
                return new Response(ex.Message, 400);
            }
            #endregion
            #region Verify user existance
            try
            {
                var exists = await _repository.AnyAsync(request.Email, cancellationToken);
                if (exists) return new Response("Email already registered", 400);
            }
            catch
            {
                return new Response("Failed while trying to verify Email", 500);
            }
            #endregion
            #region Persist data
            try
            {
                await _repository.SaveAsync(user, cancellationToken);
            }
            catch
            {
                return new Response("Failed while trying to persist data", 500);
            }
            #endregion
            #region Send activation email
            try
            {
                await _service.SendVerificationEmailAsync(user, cancellationToken);
            }
            catch
            {

                // Log event
            }
            #endregion

            return new Response("Account created with success", new ResponseData(user.Id,user.Name, user.Email));
        }
    }
}
