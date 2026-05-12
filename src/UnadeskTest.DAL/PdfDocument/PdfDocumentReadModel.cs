using Microsoft.EntityFrameworkCore;
using UnadeskTest.DAL.PdfContext;
using UnadeskTest.Domain.ReadModelProviders;

namespace UnadeskTest.DAL.PdfDocument;

public sealed class PdfDocumentReadModel(PdfDbContext context) : IPdfDocumentReadModelProvider
{
    public IAsyncEnumerable<Domain.Entities.PdfDocumentListModel> GetPdfDocumentsListAsync(int offset, int limit) =>
        context.PdfDocuments
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Select(x => new Domain.Entities.PdfDocumentListModel
            {
                FileId = x.FileId,
                FileName = x.FileName,
                CreatedAt = x.CreatedAt
            })
            .AsAsyncEnumerable();

    public Task<Domain.Entities.PdfDocument?> GetPdfDocumentOrDefaultAsync(
        Guid pdfDocumentId,
        CancellationToken cancellationToken) => 
        context.PdfDocuments.FirstOrDefaultAsync(x => x.FileId == pdfDocumentId, cancellationToken);
}