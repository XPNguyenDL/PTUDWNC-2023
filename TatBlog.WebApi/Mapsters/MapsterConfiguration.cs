using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.CategoryModel;
using TatBlog.WebApi.Models.PostModel;

namespace TatBlog.WebApi.Mapsters;

public class MapsterConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Author, AuthorDto>();
        config.NewConfig<Author, AuthorItem>()
            .Map(dest => dest.PostCount,
                src => src.Posts == null ? 0 : src.Posts.Count);

        config.NewConfig<AuthorEditModel, Author>();

        config.NewConfig<Category, CategoryDto>();
        config.NewConfig<Category, CategoryItem>()
            .Map(dest => dest.PostCount,
                src => src.Posts == null ? 0 : src.Posts.Count);

        config.NewConfig<Post, PostDto>();
        config.NewConfig<Post, PostDetail>();

        config.NewConfig<PostEditModel, Post>()
	        .Ignore(dest => dest.Id)
	        .Ignore(dest => dest.ImageUrl)
	        .Ignore(dest => dest.Tags);

        config.NewConfig<Post, PostEditModel>()
	        .Map(dest => dest.SelectedTags, src => src.Tags.Select(t => t.Name).ToList());
	}
}