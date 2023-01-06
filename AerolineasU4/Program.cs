using AerolineasU4.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<sistem21_aerolineau4Context>(x=>x.UseMySql("server=sistemas19.com;database=sistem21_aerolineau4;user=sistem21_Cesar;password=Sistemas19Cesar", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.17-mariadb")));

var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.UseFileServer();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
