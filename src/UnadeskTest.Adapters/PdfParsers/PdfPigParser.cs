using UnadeskTest.Domain.Entities;
using UnadeskTest.Domain.Services;
using PdfDocument = UglyToad.PdfPig.PdfDocument;

namespace UnadeskTest.Adapters.PdfParsers;

public sealed class PdfPigParser : IPdfParser
{
    private const string Delimiter = "\n";
    public Result<string> GetTextFromPdf(Stream pdfStream)
    {
        try
        {
            using PdfDocument document = PdfDocument.Open(pdfStream);
            return Result<string>.Success(string.Join(Delimiter, document.GetPages().Select(page => page.Text)));
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }
}