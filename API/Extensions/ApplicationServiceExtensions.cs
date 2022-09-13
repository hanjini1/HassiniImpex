using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using Core.Contracts;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

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
      services.AddDbContext<AppIdentityDbContext>(options =>
      {
        options.UseSqlite(config.GetConnectionString("IdentityConnection"));
      });
      services.AddSingleton<IConnectionMultiplexer>(c =>
      {
        var configuration = ConfigurationOptions.Parse(config.GetConnectionString("Redis"), true);
        return ConnectionMultiplexer.Connect(configuration);
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
      services.AddScoped<ITokenService, TokenService>();
      services.AddScoped<IProductRepository, ProductRepository>();
      services.AddScoped((typeof(IGenericRepository<>)), (typeof(GenericRepository<>)));
      services.AddScoped<IBasketRepository, BasketRepository>();

      services.AddCors(opt =>
      {
        opt.AddPolicy("CorsPolicy", policy =>
        {
          policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
        });
      });
      services.AddHttpContextAccessor();
      return services;
    }
  }
}