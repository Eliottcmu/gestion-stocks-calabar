using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<MongoDBService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAllOrigins",
        policy =>
        {
            policy
                .AllowAnyOrigin() // Autorise toutes les origines
                .AllowAnyMethod() // Autorise toutes les méthodes HTTP
                .AllowAnyHeader(); // Autorise tous les en-têtes
        }
    );
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run("http://localhost:5000");
