using Microsoft.EntityFrameworkCore;
using NLog.Web;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeder;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Media;
using TatBlog.WebApp.Middlewares;

namespace TatBlog.WebApp.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplicationBuilder ConfigureMVC(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();
        builder.Services.AddResponseCompression();
        return builder;
    }

    public static WebApplicationBuilder ConfigureService(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<BlogDbContext>(o =>
            o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
        builder.Services.AddScoped<IBlogRepository, BlogRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddScoped<ISubscriberRepository, SubscriberRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IDataSeeder, DataSeeder>();

        return builder;
    }

    // Cấu hình HTTP Request
    public static WebApplication UseRequestPipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Blog/Error");

            app.UseHsts();
        }

        app.UseResponseCompression();

        app.UseHttpsRedirection();
        
        app.UseStaticFiles();

        app.UseRouting();

        app.UseMiddleware<UserActivityMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseDataSeeder(
        this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        try
        {
            scope.ServiceProvider.GetRequiredService<IDataSeeder>().Initialize();
        }
        catch (Exception e)
        {
            scope.ServiceProvider.GetRequiredService<ILogger<Program>>()
                .LogError(e, "Count not insert data into database");
        }

        return app;
    }

    public static WebApplicationBuilder ConfigureNLog(
        this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
        return builder;
    }
}