using Coupon.Models.Entities.CreditAggregate;
using Coupon.Models.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using UniBook.Core.Entities.ProductAggregate;
using UniBook.Core.Enum;

namespace Coupon.Context;
public class UniBookDbContextOptions
{
    public readonly DbContextOptions<CouponDataContext> Options;
    public readonly IDbContextSeed DBContextSeed;

    public UniBookDbContextOptions(DbContextOptions<CouponDataContext> options, IDbContextSeed dbContextSeed)
    {
        Options = options;
        DBContextSeed = dbContextSeed;
    }
}

public class CouponDataContext : DbContext
{
    private readonly UniBookDbContextOptions options;
    public CouponDataContext(UniBookDbContextOptions options) : base(options.Options)
    {
        this.options = options;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        #region Credit

        builder.Entity<Credit>().Property(e => e.Description)
            .HasMaxLength(500);

        builder.Entity<Credit>().Property(e => e.Code)
            .HasMaxLength(100)
            .IsRequired();
        builder.Entity<Credit>().HasIndex(e => e.Code).IsUnique();


        builder.Entity<Credit>().HasMany(r => r.CreditUsages)
            .WithOne(r => r.Credit)
            .HasForeignKey(e => e.CreditId)
            .IsRequired();

        builder.Entity<Credit>().OwnsOne(r => r.UsageLimit);
        builder.Entity<Credit>().OwnsOne(r => r.UsageRestriction);


        builder.Entity<Credit>().Property(b => b.AllowedProducts)
            .HasField("_allowedProducts")
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        builder.Entity<Credit>().Property(e => e.AllowedProducts)
            .HasConversion(e => string.Join(',', e), value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => int.Parse(e)).ToHashSet());



        builder.Entity<Credit>().Property(b => b.ExcludedProducts)
            .HasField("_excludedProducts")
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        builder.Entity<Credit>().Property(e => e.ExcludedProducts)
            .HasConversion(e => string.Join(',', e), value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => int.Parse(e)).ToHashSet());



        builder.Entity<Credit>().Property(b => b.AllowedCategories)
            .HasField("_allowedCategories")
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        builder.Entity<Credit>().Property(e => e.AllowedCategories)
            .HasConversion(e => string.Join(',', e), value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => int.Parse(e)).ToHashSet());



        builder.Entity<Credit>().Property(b => b.ExcludedCategories)
            .HasField("_excludedCategories")
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        builder.Entity<Credit>().Property(e => e.ExcludedCategories)
            .HasConversion(e => string.Join(',', e), value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => int.Parse(e)).ToHashSet());



        builder.Entity<Credit>().Property(b => b.AllowedUsers)
            .HasField("_allowedUsers")
            .UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        builder.Entity<Credit>().Property(e => e.AllowedUsers)
            .HasConversion(e => string.Join(',', e), value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(e => int.Parse(e)).ToHashSet());

        builder.Entity<Credit>().HasDiscriminator(e => e.CreditsType)
            .HasValue<Percentage>(CreditType.Percentage)
            .HasValue<FixedProduct>(CreditType.FixedProduct)
            .HasValue<FixedCart>(CreditType.FixedCart);



        #endregion

        #region Credit Usage
        builder.Entity<CreditUsage>().HasKey(r => new { r.UserId, r.CreditId });
        #endregion

        #region FixedCart
        builder.Entity<FixedCart>().Property(e => e.Amount).IsRequired();
        #endregion

        #region FixedProduct
        builder.Entity<FixedProduct>().Property(e => e.Amount).IsRequired();
        #endregion

        #region Percentage
        builder.Entity<Percentage>().Property(e => e.Percent).IsRequired();
        #endregion

        options.DBContextSeed.Seed(builder);
    }


    #region DBSETS

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Credit> Credits { get; set; }

    #endregion
}
public static class UniBookDbContextExtensions
{
    public static DbSet<TEntityType> DbSet<TEntityType>(this CouponDataContext context)
        where TEntityType : class
    {
        return context.Set<TEntityType>();
    }
}
