using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations;

public class CategoryValidator : AbstractValidator<CategoryEditModel>
{
    public CategoryValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("Tên chủ đề không được bỏ trống")
            .MaximumLength(500).WithMessage("Tên chủ đề không được nhiều hơn 500 ký tự");

        RuleFor(s => s.Description)
            .NotEmpty()
            .WithMessage("Giới thiệu không được bỏ trống");


        RuleFor(s => s.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug không được bỏ trống")
            .MaximumLength(1000)
            .WithMessage("Slug không được nhiều hơn 512 ký tự");
    }
}