using FluentValidation;
using UnadeskTest.Host.Models;

namespace UnadeskTest.Host.Validators;

public sealed class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
{
    public UploadFileRequestValidator()
    {
        RuleFor(x => x.Files)
            .NotNull()
            .Must(files => files.Count >= 1 && files.Count <= 10)
            .WithMessage("You must upload between 1 and 10 files");
        
        RuleForEach(x => x.Files).ChildRules(file =>
        {
            file.RuleFor(f => f.FileName)
                .Must(name => name.EndsWith(".pdf") || name.EndsWith(".pdf"))
                .WithMessage("Invalid file extension");
        });
        
        RuleForEach(x => x.Files).ChildRules(file =>
        {
            file.RuleFor(f => f.Length)
                .LessThanOrEqualTo(10 * 1024 * 1024) 
                .WithMessage("The file size must be less than 10 MB");
        });
    }
}