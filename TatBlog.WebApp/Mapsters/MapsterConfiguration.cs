using Mapster;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Mapsters;

public class MapsterConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Post, PostItem>()
            .Map(dest => dest.CategoryName, src => src.Category.Name)
            .Map(dest => dest.Tags, src => src.Tags.Select(s => s.Name));

        config.NewConfig<PostFilterModel, PostQuery>()
            .Map(dest => dest.Published, src => false);

        config.NewConfig<PostEditModel, Post>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.ImageUrl)
            .Ignore(dest => dest.Tags);

        config.NewConfig<Post, PostEditModel>()
            .Map(dest => dest.SelectedTags, src => string.Join("\r\n", src.Tags.Select(s => s.Name)))
            .Ignore(dest => dest.CategoryList)
            .Ignore(dest => dest.AuthorList)
            .Ignore(dest => dest.ImageFile);

        config.NewConfig<AuthorEditModel, Author>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.ImageUrl)
            .Ignore(dest => dest.Posts);

        config.NewConfig<Author, AuthorEditModel>()
            .Ignore(dest => dest.ImageFile);

    }
}