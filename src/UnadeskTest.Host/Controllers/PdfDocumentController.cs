using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using UnadeskTest.App.GetPdfDocument;
using UnadeskTest.App.GetPdfDocumentsList;
using UnadeskTest.Domain.Entities;
using UnadeskTest.Host.Models;

namespace UnadeskTest.Host.Controllers;

[ApiController]
[Route("pdf-documents")]
[Produces("application/json")]
public sealed class PdfDocumentController(
    IRequestClient<GetPdfDocumentRequest> pdfDocumentRequestClient,
    IRequestClient<GetPdfDocumentsListRequest> pdfDocumentsListRequestClient,
    IValidator<PaginationParameters> paginationParametersValidator) 
    : ControllerBase 
{
    /// <summary>
    /// Возвращает текстовое содержимое PDF файла
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PdfDocument), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPdfDocumentAsync(Guid id, CancellationToken cancellationToken)
    {
        var response = await pdfDocumentRequestClient.GetResponse<GetPdfDocumentResponse>(
            new GetPdfDocumentRequest(id), cancellationToken);
        
        if (response.Message.PdfDocument is null)
            return NotFound();
        
        return Ok(response.Message.PdfDocument!);
    }
    
    /// <summary>
    /// Возвращает список успешно загруженных PDF файлов
    /// </summary>
    /// <param name="pageParams"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(PdfDocumentListModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPdfDocumentsListAsync([
        FromQuery] PaginationParameters pageParams, 
        CancellationToken cancellationToken)
    {
        var validationResult = await paginationParametersValidator.ValidateAsync(pageParams, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);
        
        var response = await pdfDocumentsListRequestClient.GetResponse<GetPdfDocumentsListResponse>(
            GetPdfDocumentsListRequest.Create(pageParams.Offset, pageParams.Limit), cancellationToken);
        
        return Ok(response.Message.Documents);
    }
}