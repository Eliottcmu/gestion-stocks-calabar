using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services nécessaires, comme les contrôleurs
builder.Services.AddControllers();

// Ajout du service personnalisé pour MongoDB
builder.Services.AddSingleton<MongoDBService>();

// Configuration de CORS
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

// Utilisation de CORS avant de configurer les routes
app.UseCors("AllowAllOrigins");

// Configuration du pipeline de middleware
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Configure les routes pour les contrôleurs
});

// Lancer l'application
app.Run("http://localhost:5000");
