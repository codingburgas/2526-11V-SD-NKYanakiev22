using CrmSmallBusiness.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CrmSmallBusiness.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Business> Businesses => Set<Business>();
    public DbSet<BusinessInvestment> BusinessInvestments => Set<BusinessInvestment>();
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

        builder.Entity<Business>()
            .Property(business => business.CreatedOnUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Entity<Business>()
            .HasMany(business => business.Investments)
            .WithOne(investment => investment.Business)
            .HasForeignKey(investment => investment.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BusinessInvestment>()
            .Property(investment => investment.Amount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<BusinessInvestment>()
            .Property(investment => investment.CreatedOnUtc)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
