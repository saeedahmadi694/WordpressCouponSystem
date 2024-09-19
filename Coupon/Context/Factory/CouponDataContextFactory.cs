using Coupon.Models.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Context.Factory;

public interface ICouponContextFactory
{
    CouponDataContext Create();
}
public class CouponDataContextFactory : ICouponContextFactory
{
    private readonly UniBookDbContextOptions options;
    public CouponDataContextFactory(UniBookDbContextOptions options)
    {
        this.options = options;
    }

    public CouponDataContext Create()
    {
        return new CouponDataContext(options);
    }

    public CouponDataContext CreateDbContext(string[] args)
    {
        return new CouponDataContext(options);
    }
}

public class DataContextSeed : IDbContextSeed
{
    private readonly IConfiguration configuration;
    public DataContextSeed(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public void Seed(ModelBuilder builder)
    {
      
    }
}
