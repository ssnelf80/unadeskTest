using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace UnadeskTest.DAL.PdfContext;

public abstract class BasePdfContextRepository : IDisposable
{
    protected PdfDbContext context;
    private IServiceScope _scope;
    protected IPublishEndpoint publishEndpoint;

    protected BasePdfContextRepository(IServiceScopeFactory serviceScopeFactory)
    {
        // если пробрасывать через DI dbContext и publishEndpoint оказываются в разных скоупах,
        // из-за чего не работает outbox. Создаю scope руками
        _scope = serviceScopeFactory.CreateScope();
        context = _scope.ServiceProvider.GetRequiredService<PdfDbContext>();
        publishEndpoint = _scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
    }

    public void Dispose()
    {
        context.Dispose();
        _scope.Dispose();
    }
}