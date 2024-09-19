using Coupon.Models.Entities.CreditAggregate;
using UniBook.Core.Enum;

namespace Coupon.Dtos.Credits;
public record CreditDto(int Id, string Code, decimal? Amount, int? Percentage, DateTime? ExpirationDate, bool IsActive, string? Description, CreditType CreditsType, int UsageCount)
{
    public CreditDto(Credit credit) : this(credit.Id, credit.Code, null, null, credit.ExpirationDate, credit.IsActive, credit.Description, credit.CreditsType, credit.UsageCount)
    {
        if (credit is FixedCart fixedCart) Amount = fixedCart.Amount;
        else if (credit is FixedProduct fixedProduct) Amount = fixedProduct.Amount;
        else if (credit is Percentage percentage) Percentage = percentage.Percent;
    }
}
