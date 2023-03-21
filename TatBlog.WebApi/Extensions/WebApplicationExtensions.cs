using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.Services.Timing;
using TatBlog.WebApi.Media;

namespace TatBlog.WebApi.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplicationBuilder ConfigureServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddDbContext<BlogDbContext>(option =>
            option.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<ITimeProvider, LocalTimeProvider>();
        builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
        builder.Services.AddScoped<IBlogRepository, BlogRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();


        return builder;
    }

    public static WebApplicationBuilder ConfigureCors(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(option =>
            option.AddPolicy("TatBlogApp", policyBuilder =>
                policyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()));

        return builder;
    }

    public static WebApplicationBuilder ConfigureNLog(
        this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        return builder;
    }

    public static WebApplicationBuilder ConfigureSwaggerOpenApi(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    public static WebApplication SetupRequestPipeline(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        app.UseCors("TatBlogApp");

        return app;
    }
}