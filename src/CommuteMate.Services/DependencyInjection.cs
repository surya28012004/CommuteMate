using CommuteMate.Core.Interfaces;
using CommuteMate.Core.Settings;
using CommuteMate.Data.Repositories;
using CommuteMate.Services.Bookings;
using CommuteMate.Services.Vehicles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommuteMate.Services;

/// <summary>
/// Registers service-layer dependencies into the DI container.
/// Call <c>builder.Services.AddApplicationServices()</c> from the API startup.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds application/service layer registrations. Keep registrations small and focused here.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration configuration)
    {
        // Register service layer implementations here.
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddScoped<CommuteMate.Core.Interfaces.IAuthService, AuthService>();
        services.AddSingleton<CommuteMate.Core.Interfaces.ITokenService, TokenService>();
        services.AddScoped<IVehicleRepository, VehicleRepository>();
        services.AddScoped<IRideRepository, RideRepository>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IRideService, Rides.RideService>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IDashboardRepository, DashboardRepository>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IReviewService, Reviews.ReviewService>();

        return services;
    }
}
