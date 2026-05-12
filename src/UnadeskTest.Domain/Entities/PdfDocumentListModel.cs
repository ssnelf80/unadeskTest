namespace UnadeskTest.Domain.Entities;

public sealed record PdfDocumentListModel
{
    public required Guid FileId { get; init; }
    public required string FileName { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
}