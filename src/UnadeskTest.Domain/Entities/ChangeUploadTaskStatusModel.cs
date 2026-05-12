using UnadeskTest.Domain.Enums;

namespace UnadeskTest.Domain.Entities;

public sealed record ChangeUploadTaskStatusModel(Guid TaskId, UploadTaskStatus Status, string? ErrorMessage = null)
{
    public static ChangeUploadTaskStatusModel Completed(Guid taskId)
        => new(taskId, UploadTaskStatus.Completed);
    
    public static ChangeUploadTaskStatusModel Running(Guid taskId)
        => new(taskId, UploadTaskStatus.Running);
    
    public static ChangeUploadTaskStatusModel Failed(Guid taskId, string errorMessage)
        => new(taskId, UploadTaskStatus.Failed, errorMessage);
}