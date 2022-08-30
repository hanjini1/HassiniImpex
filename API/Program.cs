using API.Errors;
using API.Helpers;
using API.Middleware;
using Core.Contracts;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Extensions;
var builder = WebApplication.CreateBuilder(args);
var _config = builder.Configuration;
// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationServices(_config);


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
app.UseStatusCodePagesWithReExecute("api/errors/{0}");

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
