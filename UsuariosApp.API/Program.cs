using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using UsuariosApp.API.Contexts;
using UsuariosApp.API.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(map => map.LowercaseUrls = true);
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer(); //Swagger
builder.Services.AddSwaggerGen(); //Swagger

//Injeção de dependência para o DataContext (Entity Framework)
builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("UsuariosApp"))
    );

//Configuração do CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()  //permissão para qualquer aplicação (origem)
              .AllowAnyMethod()  //permissão para qualquer método (POST, PUT, DELETE, GET etc)
              .AllowAnyHeader(); //permissão para enviar parametros de cabeçalho
    });
});

//Injeção de dependência para o JwtTokenHelper
builder.Services.AddSingleton<JwtTokenHelper>();

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger(); //Swagger
app.UseSwaggerUI(); //Swagger
app.MapScalarApiReference(s => s.WithTheme(ScalarTheme.BluePlanet)); //Scalar

//Habilitando a política de CORS
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();
app.Run();
