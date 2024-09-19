using UniBook.Core.Entities.ProductAggregate;
using UniBook.Core.Enum;

namespace Coupon.Models.Entities.CreditAggregate;
public class FixedProduct : Credit
{
    public decimal Amount { get; private set; }
    private FixedProduct() : base()
    {

    }
    public FixedProduct(string code, decimal amount, string? description, DateTime? expirationDate) : base(code, description, expirationDate, CreditType.FixedProduct)
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


    public decimal CalculateFixedProductDiscount(List<(decimal originalPrice, int quantity)> items)
    {
        decimal totalDiscount = 0;
        foreach (var item in items)
        {
            decimal discountPerItem = Math.Min(item.originalPrice, Amount);
            totalDiscount = discountPerItem * item.quantity;
        }
        return totalDiscount;
    }
}
