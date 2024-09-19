using Coupon.Models.Entities.CreditAggregate;
using UniBook.Core.Enum;

namespace Coupon.Dtos.Credits;

public record CreateOrEditCreditDto(int? Id, CreditType CreditsType, int Value, string Code, DateTime? ExpirationDate, bool IsActive, string? Description, CreditUsageLimitDto? UsageLimit)
{
    public CreateOrEditCreditDto(Credit credit) : this(credit.Id, credit.CreditsType, 0, credit.Code, credit.ExpirationDate, credit.IsActive,
        credit.Description, new CreditUsageLimitDto(credit))
    {
        if (credit is FixedProduct fixedProduct) { Value = (int)fixedProduct.Amount; }
        else if (credit is FixedCart fixedCart) { Value = (int)fixedCart.Amount; }
        else if (credit is Percentage percentage) { Value = (int)percentage.Percent; }
    }
}
public record CreditUsageLimitDto(int? UsageLimitPerCredit, int? UsageLimitPerUser)
{
    public CreditUsageLimitDto(Credit credit) : this(credit.UsageLimit?.UsageLimitPerCredit, credit.UsageLimit?.UsageLimitPerUser)
    {

    }
}