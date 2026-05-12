using UnadeskTest.Domain.Enums;

namespace UnadeskTest.Domain.Entities;

public sealed record UploadFileTask(
    Guid TaskId,
    Guid S3ObjectId,
    string FileName,
    UploadTaskStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? CompletedAt,
    Guid? PdfFileId,
    string? ErrorMessage
)
{
    public static UploadFileTask CreateNew(UploadFileModel uploadFileModel)
    {
        return new UploadFileTask(
            Guid.NewGuid(), 
            uploadFileModel.ObjectId, 
            uploadFileModel.FileName,
            UploadTaskStatus.Pending,
            DateTimeOffset.UtcNow,
            null, 
            null, 
            null);
    }
}