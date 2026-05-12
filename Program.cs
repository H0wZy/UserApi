using Microsoft.EntityFrameworkCore;
using user_api.cs;
using user_api.cs.Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// Adicionar camada de serviço e repositório no escopo
builder.Services.AddScoped<IGenericRepository<User>, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// TODO: AutoMapper
// builder.Services.AddAutoMapper(_)...

var app = builder.Build();
var logger = app.Logger;

// Migrations
using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();

    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Erro ao executar migration no banco {Database}", db.Database.GetDbConnection().Database);
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // TODO: Scalar
    // app.MapScalarApiReference(opt)
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();