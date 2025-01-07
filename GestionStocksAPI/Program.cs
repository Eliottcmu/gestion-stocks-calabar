using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services nécessaires, comme les contrôleurs
builder.Services.AddControllers();

// Ajout du service personnalisé pour MongoDB
builder.Services.AddSingleton<MongoDBService>();

var app = builder.Build();

// Configuration du pipeline de middleware
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Configure les routes pour les contrôleurs
});

// Lancer l'application
app.Run("http://localhost:5000");
