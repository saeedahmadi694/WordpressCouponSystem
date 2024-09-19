using System.ComponentModel.DataAnnotations.Schema;

namespace Coupon.Models.Entities.CreditAggregate;
public class CreditUsage
{
    public int UserId { get; set; }
    public int CreditId { get; set; }
    public Credit Credit { get; set; }

    public int UsageCount { get; private set; }
    public int UsageLimit { get; private set; }

    private CreditUsage()
    {
        UsageCount = 1;
        UsageLimit = 1;
    }
    public CreditUsage(int userId) : this()
    {
        UserId = userId;
    }

    internal CreditUsage IncreaseUsageCount()
    {
        UsageCount += 1;
        return this;
    }
    internal CreditUsage IncreaseUsageLimit()
    {
        UsageLimit += 1;
        return this;
    }
}

