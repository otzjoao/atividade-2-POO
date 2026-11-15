using BibliotecaApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuração padronizada de respostas de erro
builder.Services.AddProblemDetails();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Injeção de dependência: Repository registrado como Singleton
builder.Services.AddSingleton<IMaterialRepository, MaterialRepository>();

// Configuração do CORS
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

// Tratamento centralizado de exceções
app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("PermitirFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();