using UniBook.Core.Enum;

namespace Coupon.Models.Entities.CreditAggregate;
public class FixedCart : Credit
{
    public decimal Amount { get; private set; }
    private FixedCart() : base()
    {

    }
    public FixedCart(string code, decimal amount, string? description, DateTime? expirationDate) : base(code, description, expirationDate, CreditType.FixedCart)
    {
        Amount = amount;
    }

    public override Credit SetValue(object value)
    {
        if (value is decimal amount)
            Amount = amount;
        else
            throw new ArgumentException("Invalid value type for FixedCart. Expected a decimal value.");

        return this;
    }


    public decimal CalculateFixedCartDiscount(decimal totalPrice)
    {
        decimal discountAmount = Math.Min(totalPrice, Amount);
        return discountAmount;
    }

}
