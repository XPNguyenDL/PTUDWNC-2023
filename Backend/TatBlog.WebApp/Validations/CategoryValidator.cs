using FluentValidation;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations;

public class CategoryValidator : AbstractValidator<CategoryEditModel>
{
    private readonly IBlogRepository _blogRepo;

    public CategoryValidator(IBlogRepository blogRepo)
    {
        _blogRepo = blogRepo;

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

        RuleFor(s => s.UrlSlug)
            .MustAsync(async (categoryModel, slug, cancellationToken) =>
                !await _blogRepo.IsCategorySlugExistedAsync(categoryModel.Id, slug, cancellationToken))
            .WithMessage("Slug '{PropertyValue}' đã được sử dụng");
    }
}