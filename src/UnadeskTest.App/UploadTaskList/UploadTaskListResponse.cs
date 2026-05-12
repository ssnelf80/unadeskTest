using UnadeskTest.Domain.Entities;

namespace UnadeskTest.App.UploadTaskList;

public sealed record UploadTaskListResponse(IReadOnlyCollection<UploadFileTask> UploadTasks);