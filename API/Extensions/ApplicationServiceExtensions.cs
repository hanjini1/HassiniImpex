using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Contracts;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
  public static class ApplicationServiceExtensions
  {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddDbContext<StoreContext>(options =>
      {
        options.UseSqlite(config.GetConnectionString("DefaultConnection"));
      });
      services.Configure<ApiBehaviorOptions>(options =>
      {
        options.InvalidModelStateResponseFactory = actionContext =>
        {
          var errors = actionContext.ModelState
          .Where(x => x.Value.Errors.Count > 0)
          .SelectMany(x => x.Value.Errors)
          .Select(x => x.ErrorMessage).ToArray();
          var errorResponse = new ApiValidationErrorResponse()
          {
            Errors = errors
          };
          return new BadRequestObjectResult(errorResponse);
        };
      });
      services.AddScoped<IProductRepository, ProductRepository>();
      services.AddScoped((typeof(IGenericRepository<>)), (typeof(GenericRepository<>)));
      services.AddCors(opt =>
      {
        opt.AddPolicy("CorsPolicy", policy =>
        {
          policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
        });
      });
      return services;
    }
  }
}