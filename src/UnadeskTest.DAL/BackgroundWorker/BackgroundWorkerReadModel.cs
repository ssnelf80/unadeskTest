using Microsoft.EntityFrameworkCore;
using UnadeskTest.DAL.PdfContext;
using UnadeskTest.Domain.Entities;
using UnadeskTest.Domain.ReadModelProviders;

namespace UnadeskTest.DAL.BackgroundWorker;

public sealed class BackgroundWorkerReadModel(PdfDbContext context) : IBackgroundWorkerReadModelProvider
{
    public IAsyncEnumerable<UploadFileTask> GetUploadTasksListAsync(int offset, int limit) =>
        context.UploadFileTasks
            .Select(x => x)
            .OrderByDescending(x => x.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .AsAsyncEnumerable();
}