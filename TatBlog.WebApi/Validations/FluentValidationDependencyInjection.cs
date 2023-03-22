using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace TatBlog.WebApi.Validations;

public static class FluentValidationDependencyInjection
{
    public static WebApplicationBuilder ConfigureFluentValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddFluentValidationClientsideAdapters();

        builder.Services.AddValidatorsFromAssembly(
            Assembly.GetExecutingAssembly());

        return builder;
    }
}