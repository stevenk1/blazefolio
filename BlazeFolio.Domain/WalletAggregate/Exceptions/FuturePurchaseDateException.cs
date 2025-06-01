using BlazeFolio.Domain.Common;

namespace BlazeFolio.Domain.WalletAggregate.Exceptions;

public class FuturePurchaseDateException : DomainException
{
    public FuturePurchaseDateException(DateTime purchaseDate, DateTime currentDate)
        : base($"Purchase date cannot be in the future. Purchase date: {purchaseDate:d}, Current date: {currentDate:d}")
    {
        PurchaseDate = purchaseDate;
        CurrentDate = currentDate;
    }

    public DateTime PurchaseDate { get; }
    public DateTime CurrentDate { get; }
}
