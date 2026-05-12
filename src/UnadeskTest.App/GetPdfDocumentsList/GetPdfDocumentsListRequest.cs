namespace UnadeskTest.App.GetPdfDocumentsList;

public sealed record GetPdfDocumentsListRequest(int Offset, int Limit)
{
    private const int DefaultOffset = 0;
    private const int DefaultLimit = 20;
    
    public static GetPdfDocumentsListRequest Create(int? offset, int? limit) => new(
        offset ?? DefaultOffset, 
        limit ?? DefaultLimit);
}