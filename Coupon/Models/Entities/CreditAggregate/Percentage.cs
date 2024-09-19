using UniBook.Core.Enum;

namespace Coupon.Models.Entities.CreditAggregate;
public class Percentage : Credit
{
    public int Percent { get; private set; }
    private Percentage() : base()
    {

    }
    public Percentage(string code, int percent, string? description, DateTime? expirationDate) : base(code, description, expirationDate, CreditType.Percentage)
    {
        Percent = percent;
    }
    public override Credit SetValue(object value)
    {
        if (value is int percent)
            Percent = percent;
        else
            throw new ArgumentException("Invalid value type for FixedCart. Expected a decimal value.");

        return this;
    }

    public decimal CalculatePercentageDiscount(decimal totalPrice)
    {
        decimal discount = totalPrice * Percent / 100;
        return discount;
    }
}
