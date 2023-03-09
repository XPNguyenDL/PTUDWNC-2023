using TatBlog.WebApp.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.ConfigureMVC()
        .ConfigureService();
}

var app = builder.Build();
{
    app.UseRequestPipeline();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}

app.Run();
