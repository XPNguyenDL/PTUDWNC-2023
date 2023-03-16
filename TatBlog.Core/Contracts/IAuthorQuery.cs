namespace TatBlog.Core.Contracts;

public interface IAuthorQuery
{
    public string Keyword { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }       

}