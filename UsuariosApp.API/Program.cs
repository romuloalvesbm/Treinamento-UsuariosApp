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

//Inje��o de depend�ncia para o DataContext (Entity Framework)
builder.Services.AddDbContext<DataContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("UsuariosApp"))
    );

//Configura��o do CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()  //permiss�o para qualquer aplica��o (origem)
              .AllowAnyMethod()  //permiss�o para qualquer m�todo (POST, PUT, DELETE, GET etc)
              .AllowAnyHeader(); //permiss�o para enviar parametros de cabe�alho
    });
});

//Inje��o de depend�ncia para o JwtTokenHelper
builder.Services.AddSingleton<JwtTokenHelper>();

var app = builder.Build();

app.MapOpenApi();
app.UseSwagger(); //Swagger
app.UseSwaggerUI(); //Swagger
app.MapScalarApiReference(s => s.WithTheme(ScalarTheme.BluePlanet)); //Scalar

//Habilitando a pol�tica de CORS
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();
app.Run();
