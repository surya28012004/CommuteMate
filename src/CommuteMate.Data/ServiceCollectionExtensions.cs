using Microsoft.Extensions.DependencyInjection;
using CommuteMate.Core.Interfaces;
using CommuteMate.Data.Repositories;

namespace CommuteMate.Data
{
    /// <summary>
    /// Registers data layer services (repositories) into the DI container.
    /// Keep repository implementations internal; registration happens inside the same assembly.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }
    }
}
