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
        }

        public static void MapAccountEndpoints(this WebApplication app)
        {
            #region Create

            #endregion
        }
    }
}
