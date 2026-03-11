using System.Linq.Expressions;
using AceRental.Domain.Common;
using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Paprec.SIRH.RevueRem.DataLayer.Configurations;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        base.OnModelCreating(modelBuilder);
        if (modelBuilder != null)
        {
            modelBuilder.ApplyConfiguration(new PackItemConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationItemConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new QuoteConfiguration());
            
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

    public override int SaveChanges()
    {
        OnBeforeSaving();
        return base.SaveChanges();

    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        
        OnBeforeSaving();
        

        return await base.SaveChangesAsync(ct);
    }

    private void OnBeforeSaving()
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
}