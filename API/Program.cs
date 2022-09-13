using API.Errors;
using API.Helpers;
using API.Middleware;
using Core.Contracts;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Extensions;
using Microsoft.AspNetCore.Identity;
using Core.Entites.Identity;
using Infrastructure.Identity;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var _config = builder.Configuration;
// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(_config);
builder.Services.AddIdentityServices(_config);
builder.Services.AddSwaggerGen(o =>
{
  o.SwaggerDoc("v1", new OpenApiInfo { Title = "Hassini Impex", Version = "V1" });
  var securitySchema = new OpenApiSecurityScheme
  {
    Description = "JWT Auth Bearer Scheme",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",

  };
  o.AddSecurityDefinition("Bearer", securitySchema);
  o.AddSecurityRequirement(new OpenApiSecurityRequirement{
    {securitySchema, new []{"Bearer"}}
  });
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  var loggerFactory = services.GetRequiredService<ILoggerFactory>();
  try
  {
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context, loggerFactory);

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    await identityContext.Database.MigrateAsync();
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
  }
  catch (Exception ex)
  {
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occured during migration");
    throw ex;
  }
}
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {

app.UseDeveloperExceptionPage();
// }
app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/api/errors/{0}");
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
  endpoints.MapControllers();
});
app.MapControllers();


app.Run();
