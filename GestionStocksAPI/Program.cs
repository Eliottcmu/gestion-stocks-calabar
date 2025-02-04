using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Register services
        builder.Services.AddControllers();
        builder.Services.AddSingleton<MongoDBService>();
        builder.Services.AddSingleton<JwtService>();

        // Configure JWT Authentication (registered only once)
        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"])
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                };
            });

        // Add Authorization
        builder.Services.AddAuthorization();

        // Configure CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(
                "AllowAllOrigins",
                policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }
            );
        });

        var app = builder.Build();

        // Correct middleware ordering:
        // 1. Routing
        // 2. CORS
        // 3. Authentication and Authorization
        // 4. Endpoint mapping
        app.UseRouting();
        app.UseCors("AllowAllOrigins");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run("http://localhost:5000");
    }
}
