using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceTrackerApi.DataAccess.Entities;

public class PurchaseEntity
{
    public int Id { get; set; }

    [Required]
    public TransactionSourceEntity? TransactionSource { get; set; }

    public DateTime DateOfPurchase { get; set; } = DateTime.Now;
    public ICollection<PurchasedItemEntity>? PurchasedItems { get; set; }
}
