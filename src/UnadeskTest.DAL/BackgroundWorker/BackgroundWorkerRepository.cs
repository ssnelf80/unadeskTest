using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UnadeskTest.DAL.PdfContext;
using UnadeskTest.Domain.Entities;
using UnadeskTest.Domain.Repositories;

namespace UnadeskTest.DAL.BackgroundWorker;

public sealed class BackgroundWorkerRepository(IServiceScopeFactory serviceScopeFactory)
    : BasePdfContextRepository(serviceScopeFactory), IBackgroundWorkerRepository
{
    public async Task CreateUploadTasksAsync(IReadOnlyCollection<UploadFileTask> uploadTasks, CancellationToken cancellationToken)
    {
        context.UploadFileTasks.AddRange(uploadTasks);
        await publishEndpoint.PublishBatch(uploadTasks, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeStatusAsync(ChangeUploadTaskStatusModel statusModel, CancellationToken cancellationToken)
    {
        var uploadTask =
            await context.UploadFileTasks.FirstAsync(x => x.TaskId == statusModel.TaskId, cancellationToken) with
            {
                Status = statusModel.Status,
                ErrorMessage = statusModel.ErrorMessage
            };
        context.UploadFileTasks.Update(uploadTask);
        await context.SaveChangesAsync(cancellationToken);
    }
}