using UnadeskTest.Domain.Entities;

namespace UnadeskTest.Domain.Repositories;

public interface IBackgroundWorkerRepository
{
    Task CreateUploadTasksAsync(IReadOnlyCollection<UploadFileTask> uploadTasks, CancellationToken cancellationToken);
    Task ChangeStatusAsync(ChangeUploadTaskStatusModel statusModel, CancellationToken cancellationToken);
}