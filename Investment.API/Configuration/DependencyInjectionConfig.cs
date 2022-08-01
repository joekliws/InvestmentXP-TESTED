using Investment.Infra.Services;
using Investment.Infra.Repository;

namespace Investment.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddServiceScope(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IAuthService, AuthService>();
        }

        public static void AddRepositoryScope(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAssetRepository, AssetRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
