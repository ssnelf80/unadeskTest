using UnadeskTest.Domain.Entities;

namespace UnadeskTest.App.UploadPdf;

public sealed record UploadPdfRequest(IReadOnlyCollection<UploadFileModel> Files);