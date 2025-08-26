using FluentValidation;
using MoneyMarket.Application.Features.Borrowers.Commands;

namespace MoneyMarket.Application.Features.Borrowers.Validators
{

    public sealed class UploadProfilePhotoCommandValidator : AbstractValidator<UploadProfilePhotoCommand>
    {
        public UploadProfilePhotoCommandValidator()
        {
            RuleFor(x => x.File.FileName).NotEmpty();
            RuleFor(x => x.File.ContentType).NotEmpty();
            RuleFor(x => x.File.Content).NotNull();
        }
    }

    public sealed class UploadBorrowerDocumentCommandValidator : AbstractValidator<UploadBorrowerDocumentCommand>
    {
        public UploadBorrowerDocumentCommandValidator()
        {
            RuleFor(x => x.File.FileName).NotEmpty();
            RuleFor(x => x.File.ContentType).NotEmpty();
            RuleFor(x => x.File.Content).NotNull();
            RuleFor(x => x.File.Type).IsInEnum();
        }
    }

    public sealed class ReviewBorrowerProfileCommandValidator : AbstractValidator<ReviewBorrowerProfileCommand>
    {
        public ReviewBorrowerProfileCommandValidator()
        {
            RuleFor(x => x.BorrowerProfileId).NotEmpty();
            When(x => !x.Approve, () => RuleFor(x => x.Reason).NotEmpty().MaximumLength(2000));
        }
    }
}
