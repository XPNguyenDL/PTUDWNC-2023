﻿using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.CategoryModel;
using TatBlog.WebApi.Models.TagModel;

namespace TatBlog.WebApi.Models.PostModel;

public class PostDetail
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    public string MetaData { get; set; }

    public string UrlSlug { get; set; }

    public string ImageUrl { get; set; }

    public int ViewCount { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public CategoryDto Category { get; set; }

    public AuthorDto Author { get; set; }

    public IList<TagDto> Tags { get; set; }

}