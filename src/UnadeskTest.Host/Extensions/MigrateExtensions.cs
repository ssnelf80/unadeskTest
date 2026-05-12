using Microsoft.EntityFrameworkCore;
using UnadeskTest.DAL.PdfContext;

namespace UnadeskTest.Host.Extensions;

public static class MigrateExtensions
{
    public static async Task PdfDbMigrateAsync(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<PdfDbContext>();
            await context.Database.MigrateAsync(cancellationToken);
                
            logger.LogInformation("Init pdf database");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the pdf database");
        }
    }
}