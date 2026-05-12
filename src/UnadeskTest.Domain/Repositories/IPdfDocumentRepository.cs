namespace UnadeskTest.Domain.Repositories;

public interface IPdfDocumentRepository
{
    Task CreatePdfDocumentAsync(Guid uploadTaskId, string fileName, string text, CancellationToken cancellationToken);
}