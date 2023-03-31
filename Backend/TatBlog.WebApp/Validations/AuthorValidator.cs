using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations;

public class AuthorValidator : AbstractValidator<AuthorEditModel>
{
    private readonly IAuthorRepository _author;

    public AuthorValidator(IAuthorRepository author)
    {
        _author = author;

        RuleFor(post => post.FullName)
            .NotEmpty().WithMessage("Họ tên không được bỏ trống")
            .MaximumLength(500).WithMessage("Họ tên đề không được nhiều hơn 500 ký tự");

        RuleFor(s => s.Email)
            .EmailAddress().WithMessage("Không đúng định dạng email")
            .NotEmpty()
            .WithMessage("Email không được bỏ trống");

        RuleFor(s => s.Notes)
            .MaximumLength(2000).WithMessage("Ghi chú không được nhiều hơn 2000 ký tự");

        RuleFor(s => s.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug không được bỏ trống")
            .MaximumLength(1000)
            .WithMessage("Slug không được nhiều hơn 512 ký tự");

        RuleFor(s => s.UrlSlug)
            .MustAsync(async (authorModel, slug, cancellationToken) =>
                !await _author.IsExistAuthorSlugAsync(authorModel.Id, slug, cancellationToken))
            .WithMessage("Slug '{PropertyValue}' đã được sử dụng");


        When(s => s.Id == Guid.Empty, () =>
            {
                RuleFor(s => s.ImageFile)
                    .Must(s => s is { Length: > 0 })
                    .WithMessage("Bạn phải chọn ảnh đại diện");
            })
            .Otherwise(() =>
            {
                RuleFor(s => s.ImageFile)
                    .MustAsync(SetImageIfNotExist)
                    .WithMessage("Bạn phải chọn ảnh đại diện");
            });
    }

    private async Task<bool> SetImageIfNotExist(
        AuthorEditModel authorModel,
        IFormFile imageFile,
        CancellationToken cancellationToken)
    {
        var author = await _author.GetAuthorByIdAsync(authorModel.Id, cancellationToken);
        if (!string.IsNullOrWhiteSpace(author?.ImageUrl))
        {
            return true;
        }

        return imageFile is { Length: > 0 };
    }
}