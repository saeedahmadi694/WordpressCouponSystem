namespace UniBook.Application.Dtos.Credits;
public record SetOrderTransactionInput(int UserId,
                                       decimal CashUsedAmount,
                                       int PurchaseId,
                                       string PurchaseTitle,
                                       TransactionType Type,
                                       PaymentDetailInput? PaymentDetail)
{
}
public record PaymentDetailInput(decimal GatewayAmount, bool? IsSuccess, string GatewayName, int GatewayId, string? FollowCode)
{
}

public enum TransactionType
{
    CancelPurchase = 1,
    CancelOrderByVendor,
    CancelOrderBySystem
}