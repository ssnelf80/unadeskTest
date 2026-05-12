namespace UnadeskTest.Domain.Services;

public interface IS3Client
{
    Task InitializeAsync(CancellationToken cancellationToken = default);
    Task<Guid> UploadFileAsync(Stream stream, long fileLength, CancellationToken cancellationToken);
    Task<Stream> DownloadFileAsync(Guid objectId, CancellationToken cancellationToken);
}