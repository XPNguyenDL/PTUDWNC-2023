namespace TatBlog.Core.Contracts;

public interface ICategoryQuery
{
    public string Keyword { get; set; }
    public bool ShowOnMenu { get; set; }
}