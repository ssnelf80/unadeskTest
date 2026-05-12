using UnadeskTest.Domain.Entities;

namespace UnadeskTest.Domain.Services;

public interface IPdfParser
{
    Result<string> GetTextFromPdf(Stream pdfStream);
}