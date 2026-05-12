using MassTransit;
using UnadeskTest.Domain.Entities;
using UnadeskTest.Domain.Repositories;
using UnadeskTest.Domain.Services;

namespace UnadeskTest.App.ParsePdf;

public sealed class ParsePdfConsumer(
    IPublishEndpoint publishEndpoint, 
    IPdfDocumentRepository repository,
    IPdfParser pdfParser, 
    IS3Client s3Client)
    : IConsumer<UploadFileTask>
{
    public async Task Consume(ConsumeContext<UploadFileTask> context)
    {
        await publishEndpoint.Publish(
            ChangeUploadTaskStatusModel.Running(context.Message.TaskId), 
            context.CancellationToken);

        await using var fileStream =
            await s3Client.DownloadFileAsync(context.Message.S3ObjectId, context.CancellationToken);

        var parseResult = pdfParser.GetTextFromPdf(fileStream);
        
        if (parseResult.IsSuccess)
        {
            await repository.CreatePdfDocumentAsync(
                context.Message.TaskId, 
                context.Message.FileName,
                parseResult.Value, 
                context.CancellationToken);
        }
        else
        {
            await publishEndpoint.Publish(
                ChangeUploadTaskStatusModel.Failed(context.Message.TaskId, parseResult.Error), 
                context.CancellationToken);
        }
    }
}
