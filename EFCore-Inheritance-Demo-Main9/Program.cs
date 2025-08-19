using Microsoft.EntityFrameworkCore;
using NLog.Web;
using EFCore_Inheritance_Demo_Main9.Data;
using EFCore_Inheritance_Demo_Main9.Hubs;

namespace EFCore_Inheritance_Demo_Main9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder => builder
                        .SetIsOriginAllowed((host) => true) // Tillader enhver host
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()); // Nødvendigt for SignalR
            });

            // Tilføj services til Dependency Injection
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c => {
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.CustomSchemaIds(type => type.FullName);
            });

            // Konfigurer Entity Framework Core
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString));

            // Tilføj SignalR-tjenester
            builder.Services.AddSignalR();

            // **Korrekt NLog konfiguration.**
            // Det anbefales at bruge NLog.Web.AspNetCore for den nemmeste opsætning.
            builder.Logging.ClearProviders(); // Fjern standard loggers
            builder.Host.UseNLog(); // Bruger NLog's host builder til at integrere logging

            var app = builder.Build();

            // Konfigurer HTTP-anmodningspipeline
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.MapControllers();

            // Mappe SignalR Hubben til en URL
            app.MapHub<TodoHub>("/todoHub");

            // Opret databasen ved opstart, hvis den ikke eksisterer
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    var context = services.GetRequiredService<TodoContext>();
            //    context.Database.EnsureCreated();
            //}

            //app.UseNLogWeb(); // Gør NLog tilgængelig i din app-pipeline

            app.Run();
        }
    }
}
