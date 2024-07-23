using AutoMapper;
using Biblioteca.Data;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BibliotecaConnection");

builder.Services.AddDbContext<BibliotecaContext>(opts =>
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


builder.Services.AddScoped<LivroService>();
builder.Services.AddScoped<LocatarioService>();
builder.Services.AddScoped<AluguelService>();
builder.Services.AddScoped<AutorService>();


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