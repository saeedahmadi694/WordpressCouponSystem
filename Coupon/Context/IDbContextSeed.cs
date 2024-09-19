using Microsoft.EntityFrameworkCore;

namespace Coupon.Context;

public interface IDbContextSeed
{
    void Seed(ModelBuilder builder);
}
