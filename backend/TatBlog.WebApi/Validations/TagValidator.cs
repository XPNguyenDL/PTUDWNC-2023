using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models.TagModel;

namespace TatBlog.WebApi.Validations;

public class TagValidator : AbstractValidator<TagEditModel>
{
    public TagValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("Tên chủ đề không được bỏ trống")
            .MaximumLength(256).WithMessage("Tên chủ đề không được nhiều hơn 256 ký tự");

        RuleFor(s => s.Description)
            .NotEmpty().WithMessage("Giới thiệu không được bỏ trống")
			.MaximumLength(1024).WithMessage("Mô tả không được nhiều hơn 1024 ký tự");


        RuleFor(s => s.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug không được bỏ trống")
            .MaximumLength(512)
            .WithMessage("Slug không được nhiều hơn 512 ký tự");
    }
}