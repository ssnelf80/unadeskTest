using MassTransit;
using UnadeskTest.Domain.ReadModelProviders;

namespace UnadeskTest.App.GetPdfDocument;

public sealed class GetPdfDocumentConsumer(IPdfDocumentReadModelProvider provider) : IConsumer<GetPdfDocumentRequest>
{
    public async Task Consume(ConsumeContext<GetPdfDocumentRequest> context)
    {
        var result = await provider
            .GetPdfDocumentOrDefaultAsync(context.Message.PdfDocumentId, context.CancellationToken);
        
        await context.RespondAsync(new GetPdfDocumentResponse(result));
    }
}