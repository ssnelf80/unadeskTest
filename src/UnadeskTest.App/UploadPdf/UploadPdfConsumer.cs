using MassTransit;
using UnadeskTest.Domain.Entities;
using UnadeskTest.Domain.Repositories;

namespace UnadeskTest.App.UploadPdf;

public sealed class UploadPdfConsumer(IBackgroundWorkerRepository repository) : IConsumer<UploadPdfRequest>
{
    public async Task Consume(ConsumeContext<UploadPdfRequest> context)
    {
        var tasks = context.Message.Files.Select(UploadFileTask.CreateNew).ToArray();
       
        await repository.CreateUploadTasksAsync(tasks, context.CancellationToken);

        await context.RespondAsync(new UploadPdfResponse(tasks.Select(x => x.TaskId).ToArray()));
    }
}