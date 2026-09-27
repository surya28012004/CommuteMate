using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CommuteMate.Data.HealthChecks
{
    /// <summary>
    /// Health check that verifies the application's ability to connect to the configured database
    /// by using the EF Core DbContext's <c>Database.CanConnectAsync</c> method.
    /// </summary>
    public class SqlServerHealthCheck : IHealthCheck
    {
        private readonly AppDbContext _db;

        public SqlServerHealthCheck(AppDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _db.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false);
                return canConnect
                    ? HealthCheckResult.Healthy("Database reachable")
                    : HealthCheckResult.Unhealthy("Cannot connect to database");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Database check failed", ex);
            }
        }
    }
}
