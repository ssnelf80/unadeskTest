using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using UnadeskTest.App.UploadPdf;
using UnadeskTest.App.UploadTaskList;
using UnadeskTest.Domain.Entities;
using UnadeskTest.Domain.Services;
using UnadeskTest.Host.Models;

namespace UnadeskTest.Host.Controllers;

[ApiController]
[Route("upload-tasks")]
[Produces("application/json")]
public sealed class BackgroundWorkersController(
    IS3Client s3Client, 
    IRequestClient<UploadPdfRequest> uploadPdfClient,
    IRequestClient<UploadTaskListRequest> uploadTaskListClient,
    IValidator<PaginationParameters> paginationParametersValidator,
    IValidator<UploadFileRequest> uploadFileRequestValidator) 
    : ControllerBase 
{
    /// <summary>
    /// Создает задачу на загрузку PDF файла
    /// </summary>
    /// <param name="uploadFileRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(UploadPdfResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadAsync(
        [FromForm] UploadFileRequest uploadFileRequest,
        CancellationToken cancellationToken)
    {
        var validationResult = await uploadFileRequestValidator
            .ValidateAsync(uploadFileRequest, cancellationToken);
        
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        List<UploadFileModel> uploadFileModels = new(16);
        foreach (var file in uploadFileRequest.Files)
        {
            await using var fileStream = file.OpenReadStream();
            uploadFileModels.Add(new UploadFileModel(
                await s3Client.UploadFileAsync(fileStream, file.Length, cancellationToken),
                file.FileName));
        }

        var response = await uploadPdfClient.GetResponse<UploadPdfResponse>(
            new UploadPdfRequest(uploadFileModels),
            cancellationToken);
        
        return Ok(response.Message);
    }
    
    /// <summary>
    /// Список задач по загрузке PDF файлов с актуальными статусами
    /// </summary>
    /// <param name="pageParams"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(UploadFileTask), StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadTasksListAsync([FromQuery] PaginationParameters pageParams, CancellationToken cancellationToken)
    {
        var validationResult = await paginationParametersValidator.ValidateAsync(pageParams, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var response = await uploadTaskListClient.GetResponse<UploadTaskListResponse>(
             UploadTaskListRequest.Create(pageParams.Offset, pageParams.Limit), 
             cancellationToken);
        
        return Ok(response.Message.UploadTasks);
    }
}