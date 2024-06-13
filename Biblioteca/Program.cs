using AutoMapper;
using Biblioteca.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BibliotecaConnection");

builder.Services.AddDbContext<BibliotecaContext>(opts =>
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();