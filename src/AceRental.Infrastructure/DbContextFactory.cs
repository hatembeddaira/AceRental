using AceRental.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AceRental.Api"))
            .AddJsonFile("appsettings.json")
            .Build();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .UseSqlServer("Server=localhost;Database=AceRental;User Id=sa;Password=AceRental@2026!;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=True;")
            .Options;

        return new ApplicationDbContext(options);
    }
}