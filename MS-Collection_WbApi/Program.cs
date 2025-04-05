using Microsoft.EntityFrameworkCore;
using MS_Collection_WbApi.Models;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<JoyeriaMsBdContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB")));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
var app = builder.Build();

app.MapGet("/" , (HttpContext context) =>
{
    context.Response.Redirect("/Swagger/index.html", permanent: false);
});

//app.Use(async (context, next) =>
//{
//    if (context.Request.Path =="/")
//    {
//        context.Response.Redirect("/Swagger/index.html", permanent: false);
//        return;
//    }
//    await next();
//}
//);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI();
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
