using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Globalization;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations;

public class PostValidator : AbstractValidator<PostEditModel>
{
    private readonly IBlogRepository _blogRepo;

    public PostValidator(IBlogRepository blogRepo)
    {
        _blogRepo = blogRepo;

        RuleFor(post => post.Title)
            .NotEmpty().WithMessage("Chủ đề không được bỏ trống")
            .MaximumLength(500).WithMessage("Chủ đề không được nhiều hơn 500 ký tự");

        RuleFor(s => s.ShortDescription)
            .NotEmpty()
            .WithMessage("Giới thiệu không được bỏ trống"); ;

        RuleFor(s => s.Description)
            .NotEmpty()
            .WithMessage("Nội dung không được bỏ trống"); ;

        RuleFor(s => s.Meta)
            .NotEmpty()
            .WithMessage("Metadata không được bỏ trống")
            .MaximumLength(1000)
            .WithMessage("Metadata không được nhiều hơn 1000 ký tự");

        RuleFor(s => s.UrlSlug)
            .NotEmpty()
            .WithMessage("Slug không được bỏ trống")
            .MaximumLength(1000)
            .WithMessage("Slug không được nhiều hơn 1000 ký tự");

        RuleFor(s => s.UrlSlug)
            .MustAsync(async (postModel, slug, cancellationToken) =>
                !await _blogRepo.IsPostSlugExistedAsync(postModel.Id, slug, cancellationToken))
            .WithMessage("Slug '{PropertyValue}' đã được sử dụng");

        RuleFor(s => s.CategoryId)
            .NotEmpty()
            .WithMessage("Bạn phải chọn chủ đề cho bài viết");

        RuleFor(s => s.AuthorId)
            .NotEmpty()
            .WithMessage("Bạn phải chọn tác giả bài viết");

        RuleFor(s => s.SelectedTags)
            .Must(HasAtLeastOneTag)
            .WithMessage("Bạn phải nhập ít nhất một thẻ");

        When(s => s.Id == Guid.Empty, () =>
        {
            RuleFor(s => s.ImageFile)
                .Must(s => s is { Length: > 0 })
                .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
        })
            .Otherwise(() =>
            {
                RuleFor(s => s.ImageFile)
                    .MustAsync(SetImageIfNotExist)
                    .WithMessage("Bạn phải chọn hình ảnh cho bài viết");
            });
    }

    private bool HasAtLeastOneTag(PostEditModel postModel, string selectedTags)
    {
        return postModel.GetSelectTags().Any();
    }

    private async Task<bool> SetImageIfNotExist(
        PostEditModel postModel,
        IFormFile imageFile,
        CancellationToken cancellationToken)
    {
        var post = await _blogRepo.GetPostByIdAsync(postModel.Id, false, cancellationToken);
        if (!string.IsNullOrWhiteSpace(post?.ImageUrl))
        {
            return true;
        }

        return imageFile is { Length: > 0 };
    }
}