using MassTransit;
using UnadeskTest.Domain.ReadModelProviders;

namespace UnadeskTest.App.GetPdfDocumentsList;

public sealed class GetPdfDocumentsListConsumer(IPdfDocumentReadModelProvider provider) : IConsumer<GetPdfDocumentsListRequest>
{
    public async Task Consume(ConsumeContext<GetPdfDocumentsListRequest> context)
    {
        var result = await provider
            .GetPdfDocumentsListAsync(context.Message.Offset, context.Message.Limit)
            .ToArrayAsync(context.CancellationToken);
        
        await context.RespondAsync(new GetPdfDocumentsListResponse(result));
    }
}