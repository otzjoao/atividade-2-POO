using BibliotecaApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// CONFIGURAÇÃO DOS CONTROLLERS
builder.Services.AddControllers();

// CONFIGURAÇÃO DO SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// INJEÇÃO DE DEPENDÊNCIA COM SINGLETON
builder.Services.AddSingleton<
    IMaterialRepository,
    MaterialRepository
>();

// CONFIGURAÇÃO DO CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// SWAGGER
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS
app.UseHttpsRedirection();

// CORS
app.UseCors("PermitirFrontend");

// MAPEAMENTO DOS CONTROLLERS
app.MapControllers();

app.Run();