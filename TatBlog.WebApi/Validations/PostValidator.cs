using FluentValidation;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Validations;

public class PostValidator : AbstractValidator<PostEditModel>
{
    public PostValidator()
    {

		RuleFor(post => post.Title)
			.NotEmpty().WithMessage("Chủ đề không được bỏ trống")
			.MaximumLength(500).WithMessage("Chủ đề không được nhiều hơn 500 ký tự");

		RuleFor(s => s.ShortDescription)
			.NotEmpty()
			.WithMessage("Giới thiệu không được bỏ trống");

		RuleFor(s => s.Description)
			.NotEmpty()
			.WithMessage("Nội dung không được bỏ trống");

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
		

		RuleFor(s => s.CategoryId)
			.NotEmpty()
			.WithMessage("Bạn phải chọn chủ đề cho bài viết");

		RuleFor(s => s.AuthorId)
			.NotEmpty()
			.WithMessage("Bạn phải chọn tác giả bài viết");

		RuleFor(s => s.SelectedTags)
			.Must(HasAtLeastOneTag)
			.WithMessage("Bạn phải nhập ít nhất một thẻ");

	}

    private bool HasAtLeastOneTag(PostEditModel postModel, IList<string> selectedTags)
    {
	    return postModel.SelectedTags.Count() > 0;
    }
}