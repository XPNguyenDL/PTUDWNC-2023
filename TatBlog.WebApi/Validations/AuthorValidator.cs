using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations;

public class AuthorValidator : AbstractValidator<AuthorEditModel>
{
    public AuthorValidator()
    {

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

        RuleFor(a => a.JoinedDate)
            .GreaterThan(DateTime.MinValue)
            .WithMessage("Ngày tham gia không hợp lệ");

    }
}