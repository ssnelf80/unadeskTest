using MassTransit;
using UnadeskTest.Domain.ReadModelProviders;

namespace UnadeskTest.App.UploadTaskList;

public sealed class UploadTaskListConsumer(IBackgroundWorkerReadModelProvider provider) : IConsumer<UploadTaskListRequest>
{
    public async Task Consume(ConsumeContext<UploadTaskListRequest> context)
    {
        var uploadTasks = await provider
            .GetUploadTasksListAsync(context.Message.Offset, context.Message.Limit)
            .ToArrayAsync(context.CancellationToken);

        await context.RespondAsync(new UploadTaskListResponse(uploadTasks));
    }
}