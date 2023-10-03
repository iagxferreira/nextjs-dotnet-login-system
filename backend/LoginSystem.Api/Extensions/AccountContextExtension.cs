using MediatR;

namespace LoginSystem.Api.Extensions
{
    public static class AccountContextExtension
    {
        public static void AddAccountContext(this WebApplicationBuilder builder)
        {
            #region Create
            builder.Services.AddTransient<
                Core.Contexts.AccountContext.UseCases.Create.Contracts.IRepository,
                Infra.Contexts.AccountContext.UseCases.Create.Repository>();

            builder.Services.AddTransient<
                Core.Contexts.AccountContext.UseCases.Create.Contracts.IService,
                Infra.Contexts.AccountContext.UseCases.Create.Service>();

            #endregion

            #region Authenticate
            builder.Services.AddTransient<
                Core.Contexts.AccountContext.UseCases.Authenticate.Contracts.IRepository,
                Infra.Contexts.AccountContext.UseCases.Authenticate.Repository>();
            #endregion

            builder.Services.AddAuthorization(x => { x.AddPolicy("Admin", p => p.RequireRole("Admin")); });
            builder.Services.AddAuthorization(x => { x.AddPolicy("Student", p => p.RequireRole("Student")); });
            builder.Services.AddAuthorization(x => { x.AddPolicy("Premium", p => p.RequireRole("Premium")); });
        }

        public static void MapAccountEndpoints(this WebApplication app)
        {
            #region Create

            app.MapPost("/api/v1/users", async (
                LoginSystem.Core.Contexts.AccountContext.UseCases.Create.Request request,
                IRequestHandler<
                    LoginSystem.Core.Contexts.AccountContext.UseCases.Create.Request,
                    LoginSystem.Core.Contexts.AccountContext.UseCases.Create.Response> handler) => {
                    var result = await  handler.Handle(request, new CancellationToken());
                        return result.IsSuccess
                            ? Results.Created($"/api/users{result.Data?.Id}", result)
                            : Results.Json(result, statusCode: result.Status);
                }).AllowAnonymous();

            #endregion


            #region Authenticate

            app.MapPost("/api/v1/authenticate", async (
                LoginSystem.Core.Contexts.AccountContext.UseCases.Authenticate.Request request,
                IRequestHandler<
                    LoginSystem.Core.Contexts.AccountContext.UseCases.Authenticate.Request,
                    LoginSystem.Core.Contexts.AccountContext.UseCases.Authenticate.Response> handler) => {
                        var result = await handler.Handle(request, new CancellationToken());
                        if(!result.IsSuccess) return Results.Json(result, statusCode: result.Status);
                        if (result.Data is null) return Results.Json(result, statusCode: 500);

                        result.Data.Token = JwtExtension.Generate(result.Data);
                        return Results.Ok(result);
                    }).AllowAnonymous();

            app.MapGet("/api/v1/admin", async () =>
            {
                return Results.Json(new { message = "Hello admin" });
            }).RequireAuthorization("Admin");

            app.MapGet("/api/v1/student", async () =>
            {
                return Results.Json(new { message = "Hello student" });
            }).RequireAuthorization("Student");

            app.MapGet("/api/v1/premium", async () =>
            {
                return Results.Json(new { message = "Hello premium" });
            }).RequireAuthorization("Premium");

            #endregion
        }
    }
}
