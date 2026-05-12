using Microsoft.Extensions.DependencyInjection;
using UnadeskTest.DAL.PdfContext;
using UnadeskTest.Domain.Entities;
using UnadeskTest.Domain.Repositories;

namespace UnadeskTest.DAL.PdfDocument;

public sealed class PdfDocumentRepository(IServiceScopeFactory serviceScopeFactory)
    : BasePdfContextRepository(serviceScopeFactory), IPdfDocumentRepository
{
    public async Task CreatePdfDocumentAsync(Guid uploadTaskId, string fileName, string text, CancellationToken cancellationToken)
    {
        var newDocument = new Domain.Entities.PdfDocument(Guid.NewGuid(), fileName, text, DateTimeOffset.UtcNow);
        context.PdfDocuments.Add(newDocument);
        await publishEndpoint.Publish(
            ChangeUploadTaskStatusModel.Completed(uploadTaskId), 
            cancellationToken);
        
        await context.SaveChangesAsync(cancellationToken);
    }
}