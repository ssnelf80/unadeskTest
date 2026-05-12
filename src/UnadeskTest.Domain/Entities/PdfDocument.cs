namespace UnadeskTest.Domain.Entities;

public sealed record PdfDocument(Guid FileId, string FileName, string FileTextContent, DateTimeOffset CreatedAt);