namespace TatBlog.WebApi.Models.DashboardModel;

public class DashboardModel
{
	public int PostCount { get; set; }
	public int PostUnPublicCount { get; set; }
	public int CmtCount { get; set; }
	public int CmtNotVerifyCount { get; set; }
	public int AuthorCount { get; set; }
	public int SubDailyCount { get; set; }
	public int SubCount { get; set; }
}