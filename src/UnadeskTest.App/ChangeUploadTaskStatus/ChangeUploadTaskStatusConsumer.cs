using MassTransit;
using UnadeskTest.Domain.Entities;
using UnadeskTest.Domain.Repositories;

namespace UnadeskTest.App.ChangeUploadTaskStatus;

public sealed class ChangeUploadTaskStatusConsumer(IBackgroundWorkerRepository repository) : IConsumer<ChangeUploadTaskStatusModel>
{
    public Task Consume(ConsumeContext<ChangeUploadTaskStatusModel> context) 
        => repository.ChangeStatusAsync(context.Message, context.CancellationToken);
}