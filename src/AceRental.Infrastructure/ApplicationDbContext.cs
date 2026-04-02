using System.Linq.Expressions;
using AceRental.Domain.Common;
using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using AceRental.Infrastructure.Configurations;

namespace AceRental.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    //private readonly IChangeTrackerService? _changeTrackerService;

    private readonly ICurrentUserService _currentUserService;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
    ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Tables
    public DbSet<Equipment> Equipments => Set<Equipment>();
    public DbSet<Pack> Packs => Set<Pack>();
    public DbSet<PackItem> PackItems => Set<PackItem>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ReservationHistory> ReservationHistorys => Set<ReservationHistory>();
    public DbSet<ReservationItem> ReservationItems => Set<ReservationItem>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        if (modelBuilder != null)
        {
            modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
            modelBuilder.ApplyConfiguration(new PackConfiguration());
            modelBuilder.ApplyConfiguration(new PackItemConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationItemConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new QuoteConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(
                            GenerateSoftDeleteFilter(entityType.ClrType));
                }
            }
        }
        // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


    }

    // public override async Task<int> SaveChangesAsync()
    // {
    //     await OnBeforeSavingAsync();
    //     return await base.SaveChangesAsync();

    // }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        await OnBeforeSavingAsync(cancellationToken);
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        await OnBeforeSavingAsync(cancellationToken);


        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task OnBeforeSavingAsync(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? "system";

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreatedBy(userId);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetUpdatedBy(userId);
                    break;
                case EntityState.Deleted:
                    // Intercepter le DELETE → transformer en soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.SoftDelete(userId);
                    break;
            }
        }
        foreach (var entry in ChangeTracker.Entries<MinBaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreatedBy(userId);
                    break;
            }
        }

        // On récupère les factures qui vont être insérées
        var newInvoices = ChangeTracker.Entries<Invoice>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity);

        foreach (var invoice in newInvoices)
        {
            invoice.InvoiceNumber = await GetNextInvoiceSequenceValue(cancellationToken);
        }
    }
    private static LambdaExpression GenerateSoftDeleteFilter(Type entityType)
    {
        var param = Expression.Parameter(entityType, "e");
        var body = Expression.Equal(
            Expression.Property(param, nameof(BaseEntity.IsDeleted)),
            Expression.Constant(false)
        );
        return Expression.Lambda(body, param);
    }

    // Méthode helper pour appeler la séquence SQL
    private async Task<int> GetNextInvoiceSequenceValue(CancellationToken ct)
    {
        var parameter = new Microsoft.Data.SqlClient.SqlParameter
        {
            ParameterName = "@result",
            SqlDbType = System.Data.SqlDbType.Int,
            Direction = System.Data.ParameterDirection.Output
        };

        // Note : On utilise la syntaxe SQL brute pour récupérer la valeur de la séquence
        await Database.ExecuteSqlRawAsync("SET @result = NEXT VALUE FOR shared.InvoiceNumberSequence", new[] { parameter }, ct);
        var lastInvoiceNumber = (int)parameter.Value;
        if (lastInvoiceNumber == 0)
            lastInvoiceNumber = DateTime.Now.Year * 1000;

        return lastInvoiceNumber;
    }
}