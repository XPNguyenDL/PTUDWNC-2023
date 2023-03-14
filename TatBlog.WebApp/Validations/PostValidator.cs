using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Validations;

public class PostValidator : AbstractValidator<PostEditModel>
{
    private readonly IBlogRepository _blogRepo;
    
    public PostValidator(IBlogRepository blogRepo)
    {
        _blogRepo = blogRepo;

        RuleFor(s => s.Title)
            .NotEmpty()
            .MaximumLength(500);

        RuleFor(s => s.ShortDescription)
            .NotEmpty();

        RuleFor(s => s.Description)
            .NotEmpty();

        RuleFor(s => s.Meta)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(s => s.UrlSlug)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(s => s.UrlSlug)
            .MustAsync(async (postModel, slug, cancellationToken) =>
                !await _blogRepo.IsPostSlugExistedAsync(postModel.Id, slug, cancellationToken))
            .WithMessage("Slug '{PropertyValues}' đã được sử dụng");

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