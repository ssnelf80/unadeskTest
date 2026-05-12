namespace UnadeskTest.Host.Models;

public sealed class UploadFileRequest
{
    public IList<IFormFile> Files { get; init; } = [];
}