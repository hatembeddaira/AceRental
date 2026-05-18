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
    public DbSet<ReservationEquipments> ReservationEquipments => Set<ReservationEquipments>();
    public DbSet<ReservationPacks> ReservationPacks => Set<ReservationPacks>();
    public DbSet<ReservationServices> ReservationServices => Set<ReservationServices>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<QuoteLines> QuoteLines => Set<QuoteLines>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Service> Services => Set<Service>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        if (modelBuilder != null)
        {
            modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
            modelBuilder.ApplyConfiguration(new PackConfiguration());
            modelBuilder.ApplyConfiguration(new PackItemConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationEquipmentsConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationPacksConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationServicesConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new QuoteConfiguration());
            modelBuilder.ApplyConfiguration(new QuoteLinesConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(
                            GenerateSoftDeleteFilter(entityType.ClrType));
                }
                if (typeof(ArchivedEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(
                            GenerateSoftArchiveFilter(entityType.ClrType));
                }
            }
        }
        // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


    }

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

    private static LambdaExpression GenerateSoftDeleteFilter(Type entityType)
    {
        var param = Expression.Parameter(entityType, "e");
        var body = Expression.Equal(
            Expression.Property(param, nameof(BaseEntity.IsDeleted)),
            Expression.Constant(false)
        );
        return Expression.Lambda(body, param);
    }
    private static LambdaExpression GenerateSoftArchiveFilter(Type entityType)
    {
        var param = Expression.Parameter(entityType, "e");
        var body = Expression.Equal(
            Expression.Property(param, nameof(ArchivedEntity.IsArchived)),
            Expression.Constant(false)
        );
        return Expression.Lambda(body, param);
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
                case EntityState.Deleted :
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

        foreach (var entry in ChangeTracker.Entries<ArchivedEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreatedBy(userId);
                    break;
                case EntityState.Deleted :
                    entry.State = EntityState.Modified;
                    entry.Entity.SetArchivedBy(userId);
                    break;
            }
        }
        
        await InitInvoiceNumbersAsync(cancellationToken);
        await InitQuoteAsync(cancellationToken);
    }
    private async Task InitInvoiceNumbersAsync(CancellationToken cancellationToken)
    {
        // On récupère les factures qui vont être insérées
        var newInvoices = ChangeTracker.Entries<Invoice>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity);

        foreach (var invoice in newInvoices)
        {
            invoice.InvoiceNumber = await GetNextInvoiceNumberAsync(cancellationToken);
        }
    }
    private async Task InitQuoteAsync(CancellationToken cancellationToken)
    {
        // On récupère les factures qui vont être insérées
        var newQuotes = ChangeTracker.Entries<Quote>()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity);

        foreach (var quote in newQuotes)
        {
             quote.QuoteNumber = await GetNextQuoteNumberAsync(cancellationToken);
             quote.ExpiryDate = quote.CreatedAt.AddDays(15); // Par défaut, le devis est valable 15 jours
        }
    }
    private async Task<int> GetNextNumberAsync(int lastNumber, CancellationToken cancellationToken = default)
    {
        var currentYear = DateTime.Now.Year;
        var yearPrefix = currentYear * 1000;
        if (lastNumber == 0 || lastNumber < yearPrefix)
        {
            return yearPrefix + 1;
        }

        if (lastNumber >= yearPrefix && lastNumber < yearPrefix + 1000)
        {
            return lastNumber + 1;
        }

        return yearPrefix + 1;
    }
    private async Task<int> GetNextInvoiceNumberAsync(CancellationToken cancellationToken = default)
    {
        var lastNumber = await Invoices
            .AsNoTracking()
            .MaxAsync(i => (int?)i.InvoiceNumber, cancellationToken)
            .ConfigureAwait(false) ?? 0;
        return await GetNextNumberAsync(lastNumber, cancellationToken);
    }
    private async Task<int> GetNextQuoteNumberAsync(CancellationToken cancellationToken = default)
    {
        var lastNumber = await Quotes
            .AsNoTracking()
            .MaxAsync(q => (int?)q.QuoteNumber, cancellationToken)
            .ConfigureAwait(false) ?? 0;
        return await GetNextNumberAsync(lastNumber, cancellationToken);
    }
}