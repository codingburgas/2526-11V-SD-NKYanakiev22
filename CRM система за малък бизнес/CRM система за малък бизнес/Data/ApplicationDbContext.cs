using CrmSmallBusiness.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrmSmallBusiness.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Deal> Deals => Set<Deal>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Company>()
            .HasMany(company => company.Customers)
            .WithOne(customer => customer.Company)
            .HasForeignKey(customer => customer.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Customer>()
            .HasMany(customer => customer.Deals)
            .WithOne(deal => deal.Customer)
            .HasForeignKey(deal => deal.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Deal>()
            .Property(deal => deal.Value)
            .HasColumnType("decimal(18,2)");
    }
}
