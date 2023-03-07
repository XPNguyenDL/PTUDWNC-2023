using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeder;
using TatBlog.Services.Blogs;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
    builder.Services.AddScoped<IDataSeeder, DataSeeder>();

}


var app = builder.Build();
{
    // middleware để hiển thị thông báo lỗi
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Blog/Error");

        app.UseHsts();
    }
    // Để chuyển http sang https
    app.UseHttpsRedirection();

    // Phục vụ các yêu cầu liên quan tới tập tin tĩnh: images, css
    app.UseStaticFiles();

    app.UseRouting();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Blog}/{action=Index}/{id?}");
}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
    seeder.Initialize();
}

app.Run();
