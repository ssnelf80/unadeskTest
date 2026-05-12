using UnadeskTest.Domain.Entities;

namespace UnadeskTest.Domain.ReadModelProviders;

public interface IBackgroundWorkerReadModelProvider
{
    IAsyncEnumerable<UploadFileTask> GetUploadTasksListAsync(int offset, int limit);
}