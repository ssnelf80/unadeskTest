using UnadeskTest.Domain.Entities;

namespace UnadeskTest.App.GetPdfDocumentsList;

public sealed record GetPdfDocumentsListResponse(IReadOnlyCollection<PdfDocumentListModel> Documents);