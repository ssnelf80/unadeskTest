using UnadeskTest.Domain.Entities;

namespace UnadeskTest.Domain.ReadModelProviders;

public interface IPdfDocumentReadModelProvider
{
    IAsyncEnumerable<PdfDocumentListModel> GetPdfDocumentsListAsync(int offset, int limit);
    Task<PdfDocument?> GetPdfDocumentOrDefaultAsync(Guid pdfDocumentId, CancellationToken cancellationToken);
}