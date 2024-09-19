namespace UniBook.Application.Dtos.Credits;
public record SetCreditTransactionInput(int UserId,
                                       decimal CreditAmount,
                                       int CreditId,
                                       int PurchaseId,
                                       string PurchaseTitle,
                                       TransactionType Type)
{
}