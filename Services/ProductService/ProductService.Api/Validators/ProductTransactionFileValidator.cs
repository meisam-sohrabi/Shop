using FluentValidation;
using ProductService.Api.Dto;

namespace ProductService.Api.Validators
{
    public class ProductTransactionFileValidator : AbstractValidator<ProductTransactionRequestDto>
    {
        public ProductTransactionFileValidator()
        {
            RuleFor(c => c.File)
                .Must(HaveValidExtension).WithMessage("نوع فایل نامعتبر است")
                .Must(HaveValidSize).WithMessage("حجم فایل نباید بیشتر از ۱ مگابایت باشد");
        }

        private bool HaveValidExtension(IFormFile? file)
        {
            if (file == null) return false;
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            return allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
        }

        private bool HaveValidSize(IFormFile? file)
        {
            if (file == null) return false;
            return file.Length <= 1048576;
        }

    }
}
