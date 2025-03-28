
using IceSync.Domain;
using IceSync.WebApp;
using Microsoft.EntityFrameworkCore;

namespace IceSync.WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddWebAppServices(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            // Suppress EF info logs
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            builder.Logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            
            //Migrate Database
            using var scope = app.Services.CreateScope();
            await using var dbContext = scope.ServiceProvider.GetRequiredService<IceSyncDbContext>();
            await dbContext.Database.MigrateAsync();

            await app.RunAsync();
        }
    }
}
