using Microsoft.AspNetCore.Mvc;
using MyApiProject.APIs.Errors;
using MyApiProject.APIs.Helpers;
using MyApiProject.Repository.Implementations;
using MyApiProject.Repository.Interfaces;

namespace MyApiProject.APIs.Extensions;

public static class ApplicationServicesExensions
{
    public static IServiceCollection ApplicationServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenaricRepository<>), typeof(GenaricRepository<>));
        services.AddAutoMapper(typeof(MappingProfiles));
        services.Configure<ApiBehaviorOptions>(ApiBehaviorOptions =>
        {
            ApiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
            {
                var errors = actionContext.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .SelectMany(x => x.Value.Errors)
                .Select(x => x.ErrorMessage).ToArray();
                var errorResponse = new ApiValidationResponse(400, errors);
                return new BadRequestObjectResult(errorResponse);
            };
        });

        return services;
    }
}
