using Microsoft.EntityFrameworkCore;
using backend.Data;

var builder = WebApplication.CreateBuilder(args);

//CONFIGURAÇÃO DE SERVIÇOS

// Configura o Entity Framework para usar o banco de dados SQLite apontando para o arquivo gastos.db
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=gastos.db"));

// Libera a política de CORS (Cross-Origin Resource Sharing) 
// Necessário para permitir que o nosso Front-end em React consiga consultar esta API
builder.Services.AddCors();

// Adiciona o serviço de Controllers para mapear as rotas da API
builder.Services.AddControllers();

// Configura as ferramentas do Swagger para gerar a documentação interativa das rotas
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// CONFIGURAÇÃO DO PIPELINE DE REQUISIÇÕES (Middlewares)


// Ativa o Swagger visual se o ambiente for de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplica a regra de CORS liberando especificamente o endereço do nosso site em React
// Permite qualquer método (GET, POST, DELETE) e qualquer cabeçalho (Headers)
app.UseCors(opcoes => opcoes.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());

// Redireciona requisições HTTP comuns para o protocolo seguro HTTPS
app.UseHttpsRedirection();

app.UseAuthorization();

// Mapeia automaticamente os endpoints baseados nos Controllers criados
app.MapControllers();

// Dá o "Play" definitivo
app.Run();